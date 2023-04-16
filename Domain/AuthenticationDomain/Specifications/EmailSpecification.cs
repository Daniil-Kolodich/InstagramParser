using Database.Entities;
using Database.Specification;

namespace Domain.AuthenticationDomain.Specifications;

public class EmailSpecification : Specification<User>
{
    public EmailSpecification(string email) : base(u => u.Email.Equals(email))
    {
    }
}