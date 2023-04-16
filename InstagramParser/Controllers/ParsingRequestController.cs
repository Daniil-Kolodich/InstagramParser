using InstagramParser.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramParser.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ParsingRequestController : ControllerBase
{
    private IIdentityHelper _identityHelper;

    public ParsingRequestController(IIdentityHelper identityHelper)
    {
        _identityHelper = identityHelper;
    }

    [HttpPost(nameof(Post))]
    public async Task<IActionResult> Post()
    {
        return Ok(_identityHelper.UserId);
    }
}