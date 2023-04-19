using Domain.SubscriptionDomain.Models.Constants;

namespace Domain.SubscriptionDomain.Models.Requests;

public record FollowCheckRequest(string[] Source, string[] Target, SubscriptionSource SubscriptionSource, SubscriptionSource SubscriptionTarget);
