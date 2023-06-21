using Database.Entities;
using Domain.SubscriptionDomain.Models.Requests;

namespace Domain.InstagramAccountDomain;

public interface IInstagramAccountService
{
    internal Task<IEnumerable<InstagramAccount>> CreateSourceAccounts(InstagramAccountRequest[] accounts, Subscription subscription);
    internal Task<IEnumerable<InstagramAccount>> CreateTargetAccounts(InstagramAccountRequest[] accounts, Subscription subscription);

    internal Task AddFollowers(InstagramAccount parent, string[] accounts, Subscription subscription);
    internal Task AddFollowings(InstagramAccount parent, string[] accounts, Subscription subscription);
// TODO: should move getting account info and followers here
    internal void Decline(InstagramAccount account, Subscription subscription);
    internal void DeclineAll(IEnumerable<InstagramAccount> accounts, Subscription subscription);
    Task<InstagramAccountRequest> GetInstagramAccountByUserName(string userName);
}