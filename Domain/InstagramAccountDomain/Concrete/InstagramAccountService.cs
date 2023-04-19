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
        CreateInstagramAccountsFrom(string[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.From, subscription);

    public Task<IEnumerable<InstagramAccount>>
        CreateInstagramAccountsTo(string[] accounts, Subscription subscription) =>
        CreateInstagramAccounts(accounts, InstagramAccountType.To, subscription);
    
    private async Task<IEnumerable<InstagramAccount>> CreateInstagramAccounts(string[] accounts, InstagramAccountType accountType, Subscription subscription)
    {
        var result = new List<InstagramAccount>(accounts.Length);
        
        foreach (var accountId in accounts)
        {
            var account = new InstagramAccount()
            {
                InstagramId = accountId,
                InstagramAccountType = (int)accountType,
                Subscription = subscription
            };

            result.Add(await _commandRepository.AddAsync(account));
        }

        return result;
    }
}