using Database.Entities;
using Domain.InstagramAccountDomain.Constants;

namespace Domain.SharedDomain.Extensions;

public static class InstagramAccountExtensions
{
    public static bool PendingAccount(this InstagramAccount account) => !account.IsProcessed;
    public static bool OriginAccount(this InstagramAccount account) => !account.ParentId.HasValue;
    public static bool SourceAccount(this InstagramAccount account) => account.InstagramAccountType == (int)InstagramAccountType.From;
    public static bool TargetAccount(this InstagramAccount account) => account.InstagramAccountType == (int)InstagramAccountType.To;
    public static bool DeclinedAccount(this InstagramAccount account) => account.DeclinedReason.HasValue;
}