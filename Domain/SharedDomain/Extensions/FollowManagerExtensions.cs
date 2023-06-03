using Database.Entities;
using Instagram;

namespace Domain.SharedDomain.Extensions;

internal static class FollowManagerExtensions
{
    public static int ChildrenCount(this IFollowManager manager, InstagramAccount account)
    {
        if (manager is IFollowersManager)
        {
            return account.FollowersCount;
        }

        if (manager is IFollowingsManager)
        {
            return account.FollowingsCount;
        }

        return default;
    }

    public static int ChildrenPerRequest(this IFollowManager manager)
    {
        if (manager is IFollowersManager)
        {
            return 200;
        }

        if (manager is IFollowingsManager)
        {
            return 100;
        }

        return 1;
    }
}