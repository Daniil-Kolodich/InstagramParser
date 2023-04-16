using Domain.AuthenticationDomain;
using Domain.AuthenticationDomain.Models.Requests;
using InstagramParser.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace InstagramParser.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IJwtHelper _jwtHelper;
    private readonly ILoginService _loginService;
    private readonly IRegistrationService _registrationService;

    public AuthenticationController(ILoginService loginService, IRegistrationService registrationService,
        IJwtHelper jwtHelper)
    {
        _loginService = loginService;
        _registrationService = registrationService;
        _jwtHelper = jwtHelper;
    }

    [HttpGet(nameof(Get))]
    public async Task<IActionResult> Get([FromQuery] LoginUserRequest request)
    {
        var result = await _loginService.Login(request);

        return Ok(_jwtHelper.GenerateToken(result.Id.ToString()));
    }

    [HttpPost(nameof(Post))]
    public async Task<IActionResult> Post(RegisterUserRequest request)
    {
        var result = await _registrationService.Register(request);

        return Ok(_jwtHelper.GenerateToken(result.Id.ToString()));
    }
}