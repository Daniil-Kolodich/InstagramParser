using Domain.SubscriptionDomain.Models.Constants;

namespace Domain.SubscriptionDomain.Models.Requests;

public record FollowCheckRequest(InstagramAccountRequest[] Source, 
    InstagramAccountRequest[] Target, 
    SubscriptionSource SubscriptionSource, 
    SubscriptionSource SubscriptionTarget);