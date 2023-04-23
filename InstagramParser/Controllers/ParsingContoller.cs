using Domain.ParsingDomain;
using Domain.SubscriptionDomain;
using Domain.SubscriptionDomain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramParser.Controllers;

[ApiController]
// [Authorize]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly IParsingService _parsingService;

    public ParsingController(IParsingService parsingService)
    {
        _parsingService = parsingService;
        
    }

    [HttpPost(nameof(Parse))]
    public async Task<IActionResult> Parse()
    {
        var id = await _parsingService.GetSubscriptionForParsing();

        if (id is not null)
        {
            // TODO: should be fire and forget task
            await _parsingService.Parse(id.Value);
            return Ok();
        }

        return NoContent();
    }
}