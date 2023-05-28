using Database.Entities;
using Database.Repositories;
using Domain.InstagramAccountDomain;
using Domain.InstagramAccountDomain.Constants;
using Domain.ParsingDomain.Specifications;
using Domain.SubscriptionDomain.Models.Constants;
using Instagram;

namespace Domain.ParsingDomain.Concrete;

internal class ParsingService : IParsingService
{
    private static readonly Func<InstagramAccount, InstagramAccountType, bool> OriginAccount =
        (a, t) => a.ParentId is null && a.InstagramAccountType == (int)t && !a.IsProcessed;
    private static readonly Func<InstagramAccount, bool> SourceOriginAccount =
        a => OriginAccount(a, InstagramAccountType.From);
    private static readonly Func<InstagramAccount, bool> TargetOriginAccount = 
        a => OriginAccount(a, InstagramAccountType.To);


    private readonly IQueryRepository<Subscription> _querySubscriptionRepository;
    private readonly ICommandRepository<Subscription> _commandSubscriptionRepository;
    private readonly IInstagramAccountService _instagramAccountService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IInstagramManager _instagramManager;
    
    public ParsingService(IQueryRepository<Subscription> querySubscriptionRepository,
        IInstagramAccountService instagramAccountService,
        IUnitOfWork unitOfWork,
        ICommandRepository<Subscription> commandSubscriptionRepository,
        IInstagramManager instagramManager)
    {
        _querySubscriptionRepository = querySubscriptionRepository;
        _instagramAccountService = instagramAccountService;
        _unitOfWork = unitOfWork;
        _commandSubscriptionRepository = commandSubscriptionRepository;
        _instagramManager = instagramManager;
    }

    public async Task<int?> GetSubscriptionForParsing() =>
        (await _querySubscriptionRepository.GetAsync(new PendingSubscriptionsSpecification()))?.Id;

    public async Task Parse(int subscriptionId)
    {
        var subscription =
            await _querySubscriptionRepository.GetAsync(new SubscriptionByIdWithAccountsSpecification(subscriptionId));

        if (subscription is null)
        {
            return;
        }

        if (subscription.Status == (int)SubscriptionStatus.ReadyForProcessing)
        {
            // actual processing
            // parse ?:
            return;
        }

        await Process(subscription);
    }

    internal async Task Process(Subscription subscription)
    {
        (InstagramAccount Value, SubscriptionSource Source)[] accounts;
        
        {
            var sourceAccounts = subscription.InstagramAccounts
                .Where(SourceOriginAccount)
                .Select(a => (a, (SubscriptionSource)subscription.Source));

            var targetAccounts = subscription.InstagramAccounts
                .Where(TargetOriginAccount)
                .Select(a => (a, (SubscriptionSource)subscription.Target));
            
            accounts = sourceAccounts.Concat(targetAccounts).ToArray();
        }

        // TODO: Performance check later
        foreach (var (account, source) in accounts)
        {
            // can be done more async i think
            if (source == SubscriptionSource.AccountsFollowers)
            {
                var followers = await FetchChildren(account.InstagramId, CancellationToken.None, _instagramManager.GetFollowers);

                await _instagramAccountService.AddFollowers(account, followers, subscription);
            }

            if (source == SubscriptionSource.AccountsFollowings)
            {
                var followings = await FetchChildren(account.InstagramId, CancellationToken.None, _instagramManager.GetFollowings);

                await _instagramAccountService.AddFollowings(account, followings, subscription);
            }

            await _unitOfWork.SaveChanges();
        }
        
        subscription.Status = (int)SubscriptionStatus.ReadyForProcessing;
        _commandSubscriptionRepository.UpdateAsync(subscription);
        await _unitOfWork.SaveChanges();
    }

    internal async Task<string[]> FetchChildren(string instagramId, CancellationToken cancellationToken,
        Func<string, CancellationToken, Task<IEnumerable<string[]>>> fetchAction)
    {
        var childrenBatches = await fetchAction(instagramId, cancellationToken);
        return childrenBatches.SelectMany(x => x).ToArray();
    }
}