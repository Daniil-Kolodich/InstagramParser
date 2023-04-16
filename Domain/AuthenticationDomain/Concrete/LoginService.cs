using System.Net;
using Database.Entities;
using Database.Repositories;
using Domain.AuthenticationDomain.Helpers;
using Domain.AuthenticationDomain.Models.Requests;
using Domain.AuthenticationDomain.Models.Responses;
using Domain.AuthenticationDomain.Specifications;
using Domain.SharedDomain;

namespace Domain.AuthenticationDomain.Concrete;

internal class LoginService : ILoginService
{
    private DomainError LoginError => new("Email or password is invalid", HttpStatusCode.BadRequest);
    
    private readonly IQueryRepository<User> _repository;

    public LoginService(IQueryRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        var user = await _repository.GetAsync(new EmailSpecification(request.Email));

        if (user is null) throw LoginError;

        if (!PasswordHasher.VerifyPassword(request.Password, user.Password)) throw LoginError;

        return new LoginUserResponse(user.Id);
    }
}

