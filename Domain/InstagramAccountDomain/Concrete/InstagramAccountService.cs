using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain.Constants;

namespace Domain.InstagramAccountDomain.Concrete;

internal class InstagramAccountService : IInstagramAccountService
{
    private readonly ICommandRepository<InstagramAccount> _commandRepository;

    public InstagramAccountService(ICommandRepository<InstagramAccount> commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public Task<IEnumerable<InstagramAccount>>
        CreateSourceAccounts(string[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.From, subscription);

    public Task<IEnumerable<InstagramAccount>>
        CreateTargetAccounts(string[] accounts, Subscription subscription) =>
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

    private async Task AddChildren(InstagramAccount parent, InstagramAccountType accountType, string[] accounts,
        Subscription subscription)
    {
        // will this mean that old ones are deleted ?
        parent.Children = await CreateInstagramAccounts(accounts, accountType, subscription);
        parent.IsProcessed = true;
        
        _commandRepository.Update(parent);
    }

    private async Task<IEnumerable<InstagramAccount>> CreateInstagramAccounts(string[] accounts, InstagramAccountType accountType, Subscription subscription)
    {
        var result = new List<InstagramAccount>(accounts.Length);
        
        foreach (var accountId in accounts)
        {
            var account = new InstagramAccount()
            {
                InstagramId = accountId,
                UserName = String.Empty,
                InstagramAccountType = (int)accountType,
                SubscriptionId = subscription.Id
            };

            result.Add(await _commandRepository.AddAsync(account));
        }

        return result;
    }
}