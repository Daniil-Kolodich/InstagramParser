using Database.Specification;
using Domain.SubscriptionDomain.Models.Constants;

namespace Domain.ParsingDomain.Specifications;

internal class PendingSubscriptionsSpecification : Specification<Database.Entities.Subscription>
{
    public PendingSubscriptionsSpecification() : base(
        (s) => s.Status == (int)SubscriptionStatus.Pending || s.Status == (int)SubscriptionStatus.ReadyForProcessing,
        (s) => s.CreatedWhen)
    {
    }
}