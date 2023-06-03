using Database.Entities;

namespace Domain.InstagramAccountDomain;

internal interface IInstagramAccountService
{
    Task<IEnumerable<InstagramAccount>> CreateInstagramAccountsFrom(string[] accounts, Subscription subscription);
    Task<IEnumerable<InstagramAccount>> CreateInstagramAccountsTo(string[] accounts, Subscription subscription);

    internal Task AddFollowers(InstagramAccount parent, string[] accounts, Subscription subscription);
    internal Task AddFollowings(InstagramAccount parent, string[] accounts, Subscription subscription);
}