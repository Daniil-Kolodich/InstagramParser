namespace Database.Entities;

public class InstagramAccount : Entity
{
    public string InstagramId { get; set; }
    //InstagramAccountType
    public int InstagramAccountType { get; set; }
    // subscription type
    public bool IsProcessed { get; set; }
    public int? DeclinedReason { get; set; }
    
    public int? ParentId { get; set; }
    public InstagramAccount? Parent { get; set; }
    public IEnumerable<InstagramAccount> Children { get; set; }
    
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
}