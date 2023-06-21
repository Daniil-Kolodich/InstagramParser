using Database.Entities;
using Domain.InstagramAccountDomain.Constants;
using Domain.SubscriptionDomain.Models.Requests;

namespace Domain.SharedDomain.Extensions;

public static class InstagramAccountExtensions
{
    public static bool PendingAccount(this InstagramAccount account) => !account.IsProcessed;
    public static bool IsChildOf(this InstagramAccount account, IEnumerable<InstagramAccount> parents) =>
        parents.Any(p => p.Id == account.ParentId);
    public static bool OriginAccount(this InstagramAccount account) => !account.ParentId.HasValue;

    public static bool SourceAccount(this InstagramAccount account) =>
        account.InstagramAccountType == (int)InstagramAccountType.From;

    public static bool TargetAccount(this InstagramAccount account) =>
        account.InstagramAccountType == (int)InstagramAccountType.To;

    public static bool DeclinedAccount(this InstagramAccount account) => account.DeclinedReason.HasValue;

    public static InstagramAccountRequest ToResponse(this InstagramAccount account) =>
        new(account.InstagramId,
            account.UserName,
            account.FullName,
            account.FollowersCount,
            account.FollowingsCount
        );
}