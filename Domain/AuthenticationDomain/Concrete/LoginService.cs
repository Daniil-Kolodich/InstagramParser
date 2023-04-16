using Database.Entities;
using Database.Repository;
using Domain.AuthenticationDomain.Exceptions;
using Domain.AuthenticationDomain.Helpers;
using Domain.AuthenticationDomain.Models.Requests;
using Domain.AuthenticationDomain.Models.Responses;
using Domain.AuthenticationDomain.Specifications;

namespace Domain.AuthenticationDomain.Concrete;

internal class LoginService : ILoginService
{
    private readonly IQueryRepository<User> _repository;

    public LoginService(IQueryRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        var user = await _repository.GetAsync(new EmailSpecification(request.Email));

        if (user is null) throw new LoginException();

        if (!PasswordHasher.VerifyPassword(request.Password, user.Password)) throw new LoginException();

        return new LoginUserResponse(user.Id);
    }
}