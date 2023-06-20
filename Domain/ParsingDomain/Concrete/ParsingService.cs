using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain;
using Domain.ParsingDomain.Specifications;
using Domain.SharedDomain.Extensions;
using Domain.SubscriptionDomain.Models.Constants;
using Instagram;

namespace Domain.ParsingDomain.Concrete;

internal class ParsingService : IParsingService
{
    private readonly IQueryRepository<Subscription> _querySubscriptionRepository;
    private readonly ICommandRepository<Subscription> _commandSubscriptionRepository;
    private readonly ICommandRepository<InstagramAccount> _commandInstagramAccountRepository;
    private readonly IInstagramAccountService _instagramAccountService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IFollowManager _followersManager;
    private readonly IFollowManager _followingsManager;
    private readonly IUserManager _userManager;
    public ParsingService(IQueryRepository<Subscription> querySubscriptionRepository,
        IInstagramAccountService instagramAccountService,
        IUnitOfWork unitOfWork,
        ICommandRepository<Subscription> commandSubscriptionRepository, 
        IFollowersManager followersManager, 
        IFollowingsManager followingsManager, 
        IUserManager userManager, ICommandRepository<InstagramAccount> commandInstagramAccountRepository)
    {
        _querySubscriptionRepository = querySubscriptionRepository;
        _instagramAccountService = instagramAccountService;
        _unitOfWork = unitOfWork;
        _commandSubscriptionRepository = commandSubscriptionRepository;
        _followersManager = followersManager;
        _followingsManager = followingsManager;
        _userManager = userManager;
        _commandInstagramAccountRepository = commandInstagramAccountRepository;
    }

    public async Task<int?> GetSubscriptionForParsing() =>
        (await _querySubscriptionRepository.GetAsync(new PendingSubscriptionsSpecification()))?.Id;

    public async Task Parse(int subscriptionId)
    {
        var subscription =
            await _querySubscriptionRepository.GetAsync(new SubscriptionByIdWithAccountsSpecification(subscriptionId));

        if (subscription is null)
        {
            return;
        }

        if (subscription.Status == (int)SubscriptionStatus.Pending)
        {
            await PrepareNestedListsForProcessing(subscription);
            await FetchAdditionalInformation(subscription);
        
            subscription.Status = (int)SubscriptionStatus.ReadyForProcessing;
            _commandSubscriptionRepository.Update(subscription);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        if (subscription.Status != (int)SubscriptionStatus.ReadyForProcessing)
        {
            return;
        }

        var (followManager, sources, targets) = PrepareForParsing(subscription);

        //  !!! source list should be subscribed to target list !!!
        foreach (var target in targets)
        {
            var accountsLeft = sources.Where(x => !x.DeclinedAccount()).ToArray();

            var searchTask = IsChunksFaster(target, accountsLeft, followManager)
                ? SearchInChunks(accountsLeft, followManager, target)
                : SearchByName(accountsLeft, followManager, target);
            
            // will this actually update source accounts ?
            _instagramAccountService.DeclineAll(await searchTask, subscription);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    internal static async Task<InstagramAccount[]> SearchByName(InstagramAccount[] accountsLeft, IFollowManager followManager,
        InstagramAccount target)
    {
        var result = new List<InstagramAccount>();
        
        foreach (var possibleChild in accountsLeft)
        {
            var searchResults = await followManager.Search(target.InstagramId, possibleChild.UserName,
                CancellationToken.None);

            if (!searchResults.Any(x => x.Pk.Equals(possibleChild.InstagramId)))
            {
                result.Add(possibleChild);
            }
        }

        return result.ToArray();
    }

    internal static async Task<InstagramAccount[]> SearchInChunks(InstagramAccount[] accountsLeft,
        IFollowManager followManager, InstagramAccount target)
    {
        using var cts = new CancellationTokenSource();

        var result = accountsLeft.ToList();

        foreach (var chunk in await followManager.Get(target.InstagramId, null, cts.Token))
        {
            result.RemoveAll(x => chunk.Any(y => y.Pk.Equals(x.InstagramId)));

            if (!result.Any())
            {
                cts.Cancel();
            }
        }

        return result.ToArray();
    }

    internal async Task PrepareNestedListsForProcessing(Subscription subscription)
    {
        (InstagramAccount Value, SubscriptionSource Source)[] accounts;

        {
            var pendingOriginAccounts = subscription.InstagramAccounts
                .Where(a => a.PendingAccount() && a.OriginAccount()).ToList();

            var sourceAccounts = pendingOriginAccounts
                .Where(InstagramAccountExtensions.SourceAccount)
                .Select(a => (a, (SubscriptionSource)subscription.Source));

            var targetAccounts = pendingOriginAccounts
                .Where(InstagramAccountExtensions.TargetAccount)
                .Select(a => (a, (SubscriptionSource)subscription.Target));
            
            accounts = sourceAccounts.Concat(targetAccounts).ToArray();
        }

        // TODO: Performance check later
        foreach (var (account, source) in accounts)
        {
            // can be done more async i think
            if (source == SubscriptionSource.AccountsFollowers)
            {
                var followers = await FetchChildren(account.InstagramId, CancellationToken.None, _followersManager);

                await _instagramAccountService.AddFollowers(account, followers, subscription);
            }

            if (source == SubscriptionSource.AccountsFollowings)
            {
                var followings = await FetchChildren(account.InstagramId, CancellationToken.None, _followingsManager);

                await _instagramAccountService.AddFollowings(account, followings, subscription);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        
        // _commandSubscriptionRepository.Update(subscription);
        // await _unitOfWork.SaveChangesAsync();
    }

    internal async Task<string[]> FetchChildren(string instagramId, CancellationToken cancellationToken, IFollowManager manager)
    {
        var result = new List<string>();
        foreach (var chunk in await manager.Get(instagramId, null, cancellationToken))
        {
            result.AddRange(chunk.Select(x => x.Pk));            
        }

        return result.ToArray();
    }

    internal async Task FetchAdditionalInformation(Subscription subscription)
    {
        foreach (var account in subscription.InstagramAccounts)
        {
            var accountInfo = await _userManager.GetUserById(account.InstagramId, CancellationToken.None);
            account.UserName = accountInfo.Username;
            account.FollowersCount = accountInfo.FollowerCount;
            account.FollowingsCount = accountInfo.FollowingCount;
        }
        
        _commandInstagramAccountRepository.UpdateRange(subscription.InstagramAccounts);
        await _unitOfWork.SaveChangesAsync();
    }

    internal (IFollowManager, InstagramAccount[], InstagramAccount[]) PrepareForParsing(Subscription subscription)
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

        // TODO: idk will later stuff work because of this swap
        // Debug.Assert('Who cares')
        return isSearchingFollowers
            ? (_followersManager, sources.ToArray(), targets.ToArray())
            : (_followingsManager, targets.ToArray(), sources.ToArray());
    }

    
    internal static bool IsFollowersFaster(IEnumerable<InstagramAccount> influencers, IEnumerable<InstagramAccount> subs, int followersBatchSize, int followingsBatchSize)
    {
        var totalFollowers = influencers.Sum(x => x.FollowersCount);
        var totalFollowings = subs.Sum(x => x.FollowingsCount);

        var followersRequests = totalFollowers / followersBatchSize;
        var followingsRequests = totalFollowings / followingsBatchSize;

        return followersRequests < followingsRequests;
    }

    internal static bool IsChunksFaster(InstagramAccount parent, InstagramAccount[] children, IFollowManager manager)
    {
        var totalChunks = manager.ChildrenCount(parent) / manager.ChildrenPerRequest();
        var searches = children.Length;

        return totalChunks < searches;
    }
}