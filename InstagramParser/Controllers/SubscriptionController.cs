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

    [HttpGet(nameof(Id))]
    public async Task<IActionResult> Id([FromQuery] int id)
    {
        return Ok(await _subscriptionService.GetById(id));
    }
    
    [HttpGet(nameof(All))]
    public async Task<IActionResult> All()
    {
        return Ok(await _subscriptionService.GetAll());
    }
    
    [HttpPost(nameof(FollowCheck))]
    public async Task<IActionResult> FollowCheck([FromBody] FollowCheckRequest request)
    {
        return Ok(await _subscriptionService.FollowCheck(request));
    }
}