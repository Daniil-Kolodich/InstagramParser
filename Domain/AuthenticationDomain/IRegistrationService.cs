using Domain.AuthenticationDomain.Models.Requests;
using Domain.AuthenticationDomain.Models.Responses;

namespace Domain.AuthenticationDomain;

public interface IRegistrationService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request);
}