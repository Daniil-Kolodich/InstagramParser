namespace Database.Entities;

public class User : Entity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public IEnumerable<ParsingRequest> ParsingRequests { get; set; } = new List<ParsingRequest>();
}