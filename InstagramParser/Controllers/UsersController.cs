using Database.Context;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InstagranParser.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly InstagramContext _context;
    public UsersController(InstagramContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<Entity> Get()
    {
        return _context.Users.ToList();
    }
}