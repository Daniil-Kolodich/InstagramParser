using Database.Entities;
using Domain.SubscriptionDomain.Models.Requests;
using Instagram;

namespace Domain.InstagramAccountDomain;

public interface IInstagramAccountService
{
    Task<IEnumerable<InstagramAccount>> CreateSourceAccounts(InstagramAccountRequest[] accounts, Subscription subscription);
    Task<IEnumerable<InstagramAccount>> CreateTargetAccounts(InstagramAccountRequest[] accounts, Subscription subscription);

    Task AddFollowers(InstagramAccount parent, Subscription subscription);
    Task AddFollowings(InstagramAccount parent, Subscription subscription);
    
    void Decline(InstagramAccount account, Subscription subscription);
    void DeclineAll(IEnumerable<InstagramAccount> accounts, Subscription subscription);
    Task<InstagramAccountRequest> GetInstagramAccountByUserName(string userName);
    (IFollowManager, InstagramAccount[], InstagramAccount[]) PrepareForParsing(Subscription subscription);
}