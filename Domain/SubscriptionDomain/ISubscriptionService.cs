using Domain.SubscriptionDomain.Models.Requests;
using Domain.SubscriptionDomain.Models.Responses;

namespace Domain.SubscriptionDomain;

public interface ISubscriptionService
{
    Task<GetSubscriptionResponse> GetById(int requestId);
    Task<GetSubscriptionResponse> FollowCheckByAccountsFollowings(ByAccountsFollowingsRequest request);
    Task<GetSubscriptionResponse> FollowCheckByAccountsFollowers(ByAccountsFollowersRequest request);
    Task<GetSubscriptionResponse> FollowCheckByAccounts(ByAccountsRequest request);
}