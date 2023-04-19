using Domain.SubscriptionDomain.Models.Constants;

namespace Domain.SubscriptionDomain.Models.Responses;

public record GetSubscriptionResponse(int Id, SubscriptionSource Source, SubscriptionSource Target, SubscriptionStatus Status, SubscriptionType Type, string[] Accounts);
