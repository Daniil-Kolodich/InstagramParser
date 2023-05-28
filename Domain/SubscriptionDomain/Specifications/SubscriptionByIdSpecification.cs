using Database.Entities;
using Database.Specification;

namespace Domain.SubscriptionDomain.Specifications;

public class SubscriptionByIdSpecification : Specification<Subscription>
{
    public SubscriptionByIdSpecification(int id) : base (s => s.Id == id)
    {
        Include(e => e.InstagramAccounts);            
    }
}