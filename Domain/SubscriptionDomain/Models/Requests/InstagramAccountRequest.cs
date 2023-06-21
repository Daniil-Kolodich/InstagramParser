namespace Domain.SubscriptionDomain.Models.Requests;

public record InstagramAccountRequest(string Id, string UserName, string FullName, int FollowersCount, int FollowingsCount);