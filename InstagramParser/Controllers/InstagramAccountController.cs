using Domain.InstagramAccountDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramParser.Controllers;

[ApiController]
[Route("[controller]")]
public class InstagramAccountController : ControllerBase
{
    private readonly IInstagramAccountService _instagramAccountService;

    public InstagramAccountController(IInstagramAccountService instagramAccountService)
    {
        _instagramAccountService = instagramAccountService;
    }

    [HttpGet(nameof(GetByName))]
    public async Task<IActionResult> GetByName([FromQuery] string userName)
    {
        var result = await _instagramAccountService.GetInstagramAccountByUserName(userName);
        
        return Ok(result);
    }
}