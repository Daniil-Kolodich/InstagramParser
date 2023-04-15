using Database.Context;
using Database.Entities;
using Database.Repository;
using Database.Specification;
using Microsoft.AspNetCore.Mvc;

namespace InstagranParser.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _usersRepository;
    public UsersController(IRepository<User> usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet(nameof(GetName))]
    public async Task<IEnumerable<User>> GetName()
    {
        return await _usersRepository.GetAsync(new NameSpecification());
    }
    
    [HttpGet(nameof(GetAll))]
    public async Task<IEnumerable<User>> GetAll()
    {
        return await _usersRepository.GetAsync(new Specification<User>());
    }
}

public class NameSpecification : Specification<User>
{
    public NameSpecification() : base((e) => e.Id > 3)
    {
        
    }
}