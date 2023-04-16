using Database.Constants;

namespace Database.Entities;

public class ParsingRequest : Entity
{
    // perform search of which people coming from this source
    public ParsingSource ParseFrom { get; set; }
    // are subscribed to this source
    public ParsingSource ParseTo { get; set; }

    public ParsingStatus ParsingStatus { get; set; }
    public ParsingType ParsingType { get; set; }
    
    public IEnumerable<InstagramAccount> InstagramAccounts { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}