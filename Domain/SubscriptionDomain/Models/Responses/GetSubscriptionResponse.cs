using Domain.SubscriptionDomain.Models.Constants;
using Domain.SubscriptionDomain.Models.Requests;

namespace Domain.SubscriptionDomain.Models.Responses;

public record GetSubscriptionResponse(int Id, 
    SubscriptionSource Source, 
    SubscriptionSource Target, 
    SubscriptionStatus Status, 
    SubscriptionType Type, 
    InstagramAccountRequest[] SourceAccounts, 
    InstagramAccountRequest[] TargetAccounts,
    InstagramAccountRequest[] SelectedAccounts,
    DateTimeOffset CreatedAt);
