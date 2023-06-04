namespace Database.Entities;

public class InstagramAccount : Entity
{
    public string InstagramId { get; set; }
    public string UserName { get; set; }
    public int InstagramAccountType { get; set; }
    public bool IsProcessed { get; set; }


    public int? DeclinedReason { get; set; }
    
    public int FollowersCount { get; set; }
    public int FollowingsCount { get; set; }
    
    
    public int? ParentId { get; set; }
    public InstagramAccount? Parent { get; set; }
    public IEnumerable<InstagramAccount> Children { get; set; }
    
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
}