using Database.Entities;

namespace Domain.InstagramAccountDomain;

internal interface IInstagramAccountService
{
    Task<IEnumerable<InstagramAccount>> CreateSourceAccounts(string[] accounts, Subscription subscription);
    Task<IEnumerable<InstagramAccount>> CreateTargetAccounts(string[] accounts, Subscription subscription);

    Task AddFollowers(InstagramAccount parent, string[] accounts, Subscription subscription);
    Task AddFollowings(InstagramAccount parent, string[] accounts, Subscription subscription);

    void Decline(InstagramAccount account, Subscription subscription);
    void DeclineAll(IEnumerable<InstagramAccount> accounts, Subscription subscription);
}