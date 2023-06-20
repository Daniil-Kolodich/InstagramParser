namespace Domain.InstagramAccountDomain.Responses;

public record GetInstagramAccountResponse(string Id, 
    string UserName, 
    string? FullName, 
    int FollowersCount, 
    int FollowingsCount);