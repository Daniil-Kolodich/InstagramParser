namespace Instagram;

public interface IInstagramManager
{
    Task<IEnumerable<string[]>> GetFollowers(string instagramId, CancellationToken cancellationToken);
    Task<IEnumerable<string[]>> GetFollowings(string instagramId, CancellationToken cancellationToken);
}