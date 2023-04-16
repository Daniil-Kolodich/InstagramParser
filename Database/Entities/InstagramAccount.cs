using Database.Constants;

namespace Database.Entities;

public class InstagramAccount : Entity
{
    public string InstagramId { get; set; }
    public InstagramAccountType InstagramAccountType { get; set; }
    // subscription type
    public int? DeclinedReason { get; set; }
    
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
}