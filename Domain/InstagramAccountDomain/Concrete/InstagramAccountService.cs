using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain.Constants;
using Domain.SubscriptionDomain.Models.Requests;
using Instagram;

namespace Domain.InstagramAccountDomain.Concrete;

internal class InstagramAccountService : IInstagramAccountService
{
    private readonly ICommandRepository<InstagramAccount> _commandRepository;
    private readonly IUserManager _userManager;

    public InstagramAccountService(ICommandRepository<InstagramAccount> commandRepository, IUserManager userManager)
    {
        _commandRepository = commandRepository;
        _userManager = userManager;
    }

    public Task<IEnumerable<InstagramAccount>>
        CreateSourceAccounts(InstagramAccountRequest[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.From, subscription);

    public Task<IEnumerable<InstagramAccount>>
        CreateTargetAccounts(InstagramAccountRequest[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.To, subscription);

    public Task AddFollowers(InstagramAccount parent, string[] accounts, Subscription subscription) =>
        AddChildren(parent, InstagramAccountType.From, accounts, subscription);

    public Task AddFollowings(InstagramAccount parent, string[] accounts, Subscription subscription) =>
        AddChildren(parent, InstagramAccountType.To, accounts, subscription);

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

    private async Task AddChildren(InstagramAccount parent, InstagramAccountType accountType, string[] accounts,
        Subscription subscription)
    {
        // will this mean that old ones are deleted ?
        parent.Children = await CreateInstagramAccounts(
            accounts.Select(x => new InstagramAccountRequest(x, string.Empty, string.Empty, 0, 0)).ToArray(),
            accountType, subscription);
        parent.IsProcessed = true;

        _commandRepository.Update(parent);
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