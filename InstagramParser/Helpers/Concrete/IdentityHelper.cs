using System.Security.Claims;
using System.Text.Json;

namespace InstagramParser.Helpers.Concrete;

public class IdentityHelper : IIdentityHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityHelper(IHttpContextAccessor httpContextAccessor)
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