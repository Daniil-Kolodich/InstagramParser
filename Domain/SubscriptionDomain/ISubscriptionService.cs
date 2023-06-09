using Domain.SubscriptionDomain.Models.Requests;
using Domain.SubscriptionDomain.Models.Responses;

namespace Domain.SubscriptionDomain;

public interface ISubscriptionService
{
    Task<GetSubscriptionResponse> GetById(int requestId);
    Task<IEnumerable<GetSubscriptionResponse>> GetAll();
    Task<GetSubscriptionResponse> FollowCheck(FollowCheckRequest request);
}