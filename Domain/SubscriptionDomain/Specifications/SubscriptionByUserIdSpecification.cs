using Database.Entities;
using Database.Specification;

namespace Domain.SubscriptionDomain.Specifications;

public class SubscriptionByUserIdSpecification : Specification<Subscription>
{
    public SubscriptionByUserIdSpecification(int userId)
        : base(s => s.UserId == userId && !s.IsDeleted, orderBy: s => s.CreatedWhen, isDescending: false)
    {
        Include(s => s.InstagramAccounts.Where(a => !a.IsDeleted));
    }
}