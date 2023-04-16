using Database.Constants;

namespace Database.Entities;

public class Subscription : Entity
{
    // perform search of which people coming from this source
    public int Source { get; set; }
    // are subscribed to this source
    public int Target { get; set; }

    public int Status { get; set; }
    public int Type { get; set; }
    
    public IEnumerable<InstagramAccount> InstagramAccounts { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}