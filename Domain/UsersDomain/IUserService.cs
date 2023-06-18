using Domain.UsersDomain.Models.Responses;

namespace Domain.UsersDomain;

public interface IUserService
{
    Task<GetUserResponse> GetUserById(int id);
}