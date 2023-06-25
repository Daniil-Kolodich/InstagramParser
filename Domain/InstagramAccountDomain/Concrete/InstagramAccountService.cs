using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain.Constants;
using Domain.SharedDomain.Extensions;
using Domain.SubscriptionDomain.Models.Constants;
using Domain.SubscriptionDomain.Models.Requests;
using Instagram;
using Instagram.Concrete;


namespace Domain.InstagramAccountDomain.Concrete;

internal class InstagramAccountService : IInstagramAccountService
{
    private readonly ICommandRepository<InstagramAccount> _commandRepository;
    private readonly IUserManager _userManager;
    private readonly IFollowManager _followersManager;
    private readonly IFollowManager _followingsManager;

    public InstagramAccountService(ICommandRepository<InstagramAccount> commandRepository, IUserManager userManager,
        IFollowersManager followersManager, IFollowingsManager followingsManager)
    {
        _commandRepository = commandRepository;
        _userManager = userManager;
        _followersManager = followersManager;
        _followingsManager = followingsManager;
    }

    public Task<IEnumerable<InstagramAccount>>
        CreateSourceAccounts(InstagramAccountRequest[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.From, subscription);

    public Task<IEnumerable<InstagramAccount>>
        CreateTargetAccounts(InstagramAccountRequest[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.To, subscription);

    public Task AddFollowers(InstagramAccount parent, Subscription subscription) =>
        AddChildren(parent, InstagramAccountType.From, _followersManager, subscription);

    public Task AddFollowings(InstagramAccount parent, Subscription subscription) =>
        AddChildren(parent, InstagramAccountType.To, _followingsManager, subscription);

    public void Decline(InstagramAccount account, Subscription subscription)
    {
        account.DeclinedReason = subscription.Type;

        _commandRepository.Update(account);
    }

    public void DeclineAll(IEnumerable<InstagramAccount> accounts, Subscription subscription)
    {
        var instagramAccounts = accounts as InstagramAccount[] ?? accounts.ToArray();

        if (!instagramAccounts.Any())
        {
            return;
        }

        foreach (var account in instagramAccounts)
        {
            account.DeclinedReason = subscription.Type;
        }

        _commandRepository.UpdateRange(instagramAccounts);
    }

    public async Task<InstagramAccountRequest> GetInstagramAccountByUserName(string userName)
    {
        var result = await _userManager.GetUserByUsername(userName, CancellationToken.None);

        return new InstagramAccountRequest(result.Pk,
            result.Username,
            result.FullName,
            result.FollowerCount,
            result.FollowingCount);
    }
    
    public (IFollowManager, InstagramAccount[], InstagramAccount[]) PrepareForParsing(Subscription subscription)
    {
        var sources = subscription
            .InstagramAccounts
            .Where(a => a.OriginAccount() && a.SourceAccount())
            .ToList();

        var targets = subscription
            .InstagramAccounts
            .Where(a => a.OriginAccount() && a.TargetAccount())
            .ToList();


        if (subscription.Source != (int)SubscriptionSource.AccountsList)
        {
            sources = subscription.InstagramAccounts.Where(x => x.IsChildOf(sources)).ToList();
        }

        if (subscription.Target != (int)SubscriptionSource.AccountsList)
        {
            targets = subscription.InstagramAccounts.Where(x => x.IsChildOf(targets)).ToList();
        }

        var isSearchingFollowers = IsFollowersFaster(sources, targets,
            _followersManager.ChildrenPerRequest(), _followingsManager.ChildrenPerRequest());

        return isSearchingFollowers
            ? (_followersManager, sources.ToArray(), targets.ToArray())
            : (_followingsManager, targets.ToArray(), sources.ToArray());
    }

    internal static bool IsFollowersFaster(IEnumerable<InstagramAccount> influencers,
        IEnumerable<InstagramAccount> subs, int followersBatchSize, int followingsBatchSize)
    {
        var totalFollowers = influencers.Sum(x => x.FollowersCount);
        var totalFollowings = subs.Sum(x => x.FollowingsCount);

        var followersRequests = totalFollowers / followersBatchSize;
        var followingsRequests = totalFollowings / followingsBatchSize;

        return followersRequests < followingsRequests;
    }
    
    private async Task AddChildren(InstagramAccount parent, InstagramAccountType accountType, IFollowManager manager,
        Subscription subscription)
    {
        var children = await FetchChildren(parent.InstagramId, CancellationToken.None, manager);
        var requests = await GetFilledRequests(children, _userManager);

        parent.Children = await CreateInstagramAccounts(requests, accountType, subscription);
        parent.IsProcessed = true;

        _commandRepository.Update(parent);
    }

    internal static async Task<InstagramAccountRequest[]> GetFilledRequests(UserShort[] accounts, IUserManager manager)
    {
        var result = new List<InstagramAccountRequest>();

        foreach (var account in accounts)
        {
            var accountInfo = await manager.GetUserById(account.Pk, CancellationToken.None);
            result.Add(new InstagramAccountRequest(accountInfo.Pk,
                accountInfo.Username,
                accountInfo.FullName,
                accountInfo.FollowerCount,
                accountInfo.FollowingCount));
        }

        return result.ToArray();
    }

    internal async Task<UserShort[]> FetchChildren(string instagramId, CancellationToken cancellationToken,
        IFollowManager manager)
    {
        var result = new List<UserShort>();
        foreach (var chunk in await manager.Get(instagramId, null, cancellationToken))
        {
            result.AddRange(chunk);
        }

        return result.ToArray();
    }

    private async Task<IEnumerable<InstagramAccount>> CreateInstagramAccounts(InstagramAccountRequest[] accounts,
        InstagramAccountType accountType, Subscription subscription)
    {
        var result = new List<InstagramAccount>(accounts.Length);

        foreach (var account in accounts)
        {
            var entity = new InstagramAccount()
            {
                InstagramId = account.Id,
                FullName = account.FullName,
                UserName = account.UserName,
                FollowingsCount = account.FollowingsCount,
                FollowersCount = account.FollowersCount,
                InstagramAccountType = (int)accountType,
                SubscriptionId = subscription.Id
            };

            result.Add(await _commandRepository.AddAsync(entity));
        }

        return result;
    }
}