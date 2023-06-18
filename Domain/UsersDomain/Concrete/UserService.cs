using System.Net;
using Database.Entities;
using Database.Repositories;
using Domain.SharedDomain;
using Domain.UsersDomain.Models.Responses;
using Domain.UsersDomain.Specifications;

namespace Domain.UsersDomain.Concrete;

public class UserService : IUserService
{
    private DomainError UserNotFound => new("User not found", HttpStatusCode.NotFound);
    private readonly IQueryRepository<User> _queryRepository;

    public UserService(IQueryRepository<User> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<GetUserResponse> GetUserById(int id)
    {
        var result = await _queryRepository.GetAsync(new UserByIdSpecification(id));

        if (result is null) throw UserNotFound;
        
        return new GetUserResponse(result.UserName);
    }
}