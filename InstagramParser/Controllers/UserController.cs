using Domain.ParsingDomain;
using Domain.SharedDomain;
using Domain.UsersDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramParser.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IIdentityService _identityService;

    public UserController(IUserService userService, IIdentityService identityService)
    {
        _userService = userService;
        _identityService = identityService;
    }

    [HttpGet(nameof(GetById))]
    public async Task<IActionResult> GetById()
    {
        var result = await _userService.GetUserById(_identityService.UserId);

        return Ok(result);
    }
}