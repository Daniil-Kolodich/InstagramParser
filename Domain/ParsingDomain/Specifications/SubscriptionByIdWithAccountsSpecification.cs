using Database.Specification;

namespace Domain.ParsingDomain.Specifications;

internal class SubscriptionByIdWithAccountsSpecification : Specification<Database.Entities.Subscription>
{
    public SubscriptionByIdWithAccountsSpecification(int id) : base((s) => s.Id == id)
    {
        // i need to also don't include deleted accounts
        Include((s) => s.InstagramAccounts.Where(a => !a.IsDeleted));
    }
}