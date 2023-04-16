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
    
    [HttpPost(nameof(FollowCheckByAccountsFollowers))]
    public async Task<IActionResult> FollowCheckByAccountsFollowers([FromQuery] ByAccountsFollowersRequest request)
    {
        return Ok(await _subscriptionService.FollowCheckByAccountsFollowers(request));
    }
    
    [HttpPost(nameof(FollowCheckByAccountsFollowings))]
    public async Task<IActionResult> FollowCheckByAccountsFollowings([FromQuery] ByAccountsFollowingsRequest request)
    {
        return Ok(await _subscriptionService.FollowCheckByAccountsFollowings(request));
    }
    
    [HttpPost(nameof(FollowCheckByAccounts))]
    public async Task<IActionResult> FollowCheckByAccounts([FromQuery] ByAccountsRequest request)
    {
        return Ok(await _subscriptionService.FollowCheckByAccounts(request));
    }
}