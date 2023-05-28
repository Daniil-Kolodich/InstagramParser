namespace Instagram.Concrete;

public class InstagramManager : IInstagramManager
{
    public Task<IEnumerable<string[]>> GetFollowers(string instagramId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string[]>> GetFollowings(string instagramId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}