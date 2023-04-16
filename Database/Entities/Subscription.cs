using Database.Constants;

namespace Database.Entities;

public class Subscription : Entity
{
    // perform search of which people coming from this source
    public ParsingSource Source { get; set; }
    // are subscribed to this source
    public ParsingSource Target { get; set; }

    public ParsingStatus Status { get; set; }
    public ParsingType Type { get; set; }
    
    public IEnumerable<InstagramAccount> InstagramAccounts { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}