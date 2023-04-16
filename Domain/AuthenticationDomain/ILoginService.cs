using Domain.AuthenticationDomain.Models.Requests;
using Domain.AuthenticationDomain.Models.Responses;

namespace Domain.AuthenticationDomain;

public interface ILoginService
{
    Task<LoginUserResponse> Login(LoginUserRequest request);
}