using Database.Entities;
using Domain.InstagramAccountDomain.Constants;
using Domain.InstagramAccountDomain.Responses;

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

    public static GetInstagramAccountResponse ToResponse(this InstagramAccount account) =>
        new GetInstagramAccountResponse(account.InstagramId,
            account.UserName,
            // TODO: should i add FullName to entity?
            null,
            account.FollowersCount,
            account.FollowingsCount
        );
}