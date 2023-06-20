using Database.Entities;
using Domain.SubscriptionDomain.Models.Constants;

namespace Domain.SharedDomain.Extensions;

public static class SubscriptionExtensions
{
    public static IEnumerable<InstagramAccount> SourceOriginAccounts(this Subscription subscription) =>
        subscription.InstagramAccounts.Where(a => a.SourceAccount() && a.OriginAccount());
    public static IEnumerable<InstagramAccount> TargetOriginAccounts(this Subscription subscription) =>
        subscription.InstagramAccounts.Where(a => a.TargetAccount() && a.OriginAccount());

    public static IEnumerable<InstagramAccount> MatchingAccounts(this Subscription subscription)
    {
        if ((SubscriptionStatus)subscription.Status != SubscriptionStatus.Completed)
        {
            return Enumerable.Empty<InstagramAccount>();
        }
        
        var parents = subscription.TargetOriginAccounts().ToList();
        if ((SubscriptionSource)subscription.Target != SubscriptionSource.AccountsList)
        {
            var children = subscription.InstagramAccounts.Where(a => a.IsChildOf(parents)).ToList();
            parents.Clear();
            parents.AddRange(children);
        }

        return subscription.InstagramAccounts.Where(a => a.IsChildOf(parents) && !a.DeclinedAccount());
    }
}