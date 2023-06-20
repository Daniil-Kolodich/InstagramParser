using System.Net;
using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain;
using Domain.SharedDomain;
using Domain.SharedDomain.Extensions;
using Domain.SubscriptionDomain.Models.Constants;
using Domain.SubscriptionDomain.Models.Requests;
using Domain.SubscriptionDomain.Models.Responses;
using Domain.SubscriptionDomain.Specifications;

namespace Domain.SubscriptionDomain.Concrete;

internal class SubscriptionService : ISubscriptionService
{
    private DomainError SubscriptionSaveError =>
        new("Unable to create subscription", HttpStatusCode.InternalServerError);

    private DomainError SubscriptionNotFoundError(int id) =>
        new($"Unable to get subscription by id {id}", HttpStatusCode.NotFound);

    private readonly IQueryRepository<Subscription> _queryRepository;
    private readonly ICommandRepository<Subscription> _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IInstagramAccountService _instagramAccountService;
    private readonly IIdentityService _identityService;

    public SubscriptionService(IQueryRepository<Subscription> queryRepository,
        ICommandRepository<Subscription> commandRepository, IIdentityService identityService, IUnitOfWork unitOfWork,
        IInstagramAccountService instagramAccountService)
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

        return CreateResponse(entity);
    }

    public async Task<IEnumerable<GetSubscriptionResponse>> GetAll()
    {
        var results =
            await _queryRepository.GetAllAsync(new SubscriptionByUserIdSpecification(_identityService.UserId));

        return results.Select(CreateResponse);
    }

    public async Task<GetSubscriptionResponse> FollowCheck(FollowCheckRequest request)
    {
        // TODO: use transaction when saving nested objects separately
        var subscription = new Subscription()
        {
            Source = (int)request.SubscriptionSource,
            Target = (int)request.SubscriptionTarget,
            Status = (int)SubscriptionStatus.Pending,
            Type = (int)SubscriptionType.Follow,
            UserId = _identityService.UserId
        };

        var sourceAccounts = await _instagramAccountService.CreateSourceAccounts(request.Source, subscription);
        var targetAccounts = await _instagramAccountService.CreateTargetAccounts(request.Target, subscription);

        subscription.InstagramAccounts = sourceAccounts.Concat(targetAccounts).ToList();

        var entity = await _commandRepository.AddAsync(subscription);

        if (!await _unitOfWork.SaveChangesAsync())
        {
            throw SubscriptionSaveError;
        }

        return CreateResponse(entity);
    }

    private static GetSubscriptionResponse CreateResponse(Subscription entity)
    {
        return new GetSubscriptionResponse(
            entity.Id,
            (SubscriptionSource)entity.Source,
            (SubscriptionSource)entity.Target,
            (SubscriptionStatus)entity.Status,
            (SubscriptionType)entity.Type,
            entity.SourceOriginAccounts().Select(InstagramAccountExtensions.ToResponse).ToArray(),
            entity.TargetOriginAccounts().Select(InstagramAccountExtensions.ToResponse).ToArray(),
            entity.MatchingAccounts().Select(InstagramAccountExtensions.ToResponse).ToArray(),
            entity.CreatedWhen
        );
    }
}