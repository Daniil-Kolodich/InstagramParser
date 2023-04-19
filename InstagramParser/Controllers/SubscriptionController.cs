using Domain.SubscriptionDomain;
using Domain.SubscriptionDomain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramParser.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    
    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        return Ok(await _subscriptionService.GetById(id));
    }
    
    [HttpPost(nameof(FollowCheck))]
    public async Task<IActionResult> FollowCheck([FromBody] FollowCheckRequest request)
    {
        return Ok(await _subscriptionService.FollowCheck(request));
    }
}