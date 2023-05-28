using System.Net;
using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain;
using Domain.SharedDomain;
using Domain.SubscriptionDomain.Models.Constants;
using Domain.SubscriptionDomain.Models.Requests;
using Domain.SubscriptionDomain.Models.Responses;
using Domain.SubscriptionDomain.Specifications;

namespace Domain.SubscriptionDomain.Concrete;

internal class SubscriptionService : ISubscriptionService
{
    private DomainError SubscriptionSaveError => new("Unable to create subscription", HttpStatusCode.InternalServerError);
    private DomainError SubscriptionNotFoundError(int id) => new($"Unable to get subscription by id {id}", HttpStatusCode.NotFound);

    private readonly IQueryRepository<Subscription> _queryRepository;
    private readonly ICommandRepository<Subscription> _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IInstagramAccountService _instagramAccountService;
    private readonly IIdentityService _identityService;

    public SubscriptionService(IQueryRepository<Subscription> queryRepository, ICommandRepository<Subscription> commandRepository, IIdentityService identityService, IUnitOfWork unitOfWork, IInstagramAccountService instagramAccountService)
    {
        _queryRepository = queryRepository;
        _commandRepository = commandRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
        _instagramAccountService = instagramAccountService;
    }

    public async Task<GetSubscriptionResponse> GetById(int id)
    {
        var entity = await _queryRepository.GetAsync(new SubscriptionByIdSpecification(id));

        if (entity is null)
        {
            throw SubscriptionNotFoundError(id);
        }
        
        return new GetSubscriptionResponse(
            entity.Id,
            (SubscriptionSource) entity.Source,
            (SubscriptionSource) entity.Target,
            (SubscriptionStatus) entity.Status,
            (SubscriptionType) entity.Type,
            entity.InstagramAccounts.Select(a => a.InstagramId).ToArray());
    }

    public async Task<GetSubscriptionResponse> FollowCheck(FollowCheckRequest request)
    {
        // TODO: use transaction when saving nested objects separately
        var subscription = new Subscription()
        {
            Source = (int)request.SubscriptionSource,
            Target = (int)request.SubscriptionTarget,
            Status = (int)GetSubscriptionStatus(request.SubscriptionSource, request.SubscriptionTarget),
            Type = (int)SubscriptionType.Follow,
            UserId = _identityService.UserId
        };

        var sourceAccounts = await _instagramAccountService.CreateInstagramAccountsFrom(request.Source, subscription);
        var targetAccounts = await _instagramAccountService.CreateInstagramAccountsTo(request.Target, subscription);

        subscription.InstagramAccounts = sourceAccounts.Concat(targetAccounts).ToList();

        var entity = await _commandRepository.AddAsync(subscription);

        if (!await _unitOfWork.SaveChanges())
        {
            throw SubscriptionSaveError;
        }

        return new GetSubscriptionResponse(
            entity.Id,
            (SubscriptionSource) entity.Source,
            (SubscriptionSource) entity.Target,
            (SubscriptionStatus) entity.Status,
            (SubscriptionType) entity.Type,
            entity.InstagramAccounts.Select(a => a.InstagramId).ToArray());
    }

    internal static SubscriptionStatus GetSubscriptionStatus(SubscriptionSource source, SubscriptionSource target) =>
        source == SubscriptionSource.AccountsList && target == SubscriptionSource.AccountsList
            ? SubscriptionStatus.ReadyForProcessing
            : SubscriptionStatus.Pending;
}