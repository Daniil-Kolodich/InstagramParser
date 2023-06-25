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

        if ((SubscriptionSource)subscription.Source != SubscriptionSource.AccountsList)
        {
            var parents = subscription.SourceOriginAccounts();

            return subscription.InstagramAccounts.Where(a => a.IsChildOf(parents) && !a.DeclinedAccount());
        }

        return subscription.SourceOriginAccounts().Where(a => !a.DeclinedAccount());
    }
}