using Database.Entities;
using Database.Specification;

namespace Domain.UsersDomain.Specifications;

public class UserByIdSpecification : Specification<User>
{
    public UserByIdSpecification(int id) : base(u => u.Id == id && !u.IsDeleted) { }
}