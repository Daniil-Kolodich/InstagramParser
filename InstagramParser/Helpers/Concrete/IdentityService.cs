using System.Security.Claims;
using System.Text.Json;
using Domain.SharedDomain;

namespace InstagramParser.Helpers.Concrete;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId {
        get
        {
            var userIdClaim = (_httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity)?.FindFirst(JwtHelper.UserIdClaim) ?? null;

            if (userIdClaim is null)
            {
                throw new UnauthorizedAccessException();
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException();
            }

            return userId;
        }
    }
}