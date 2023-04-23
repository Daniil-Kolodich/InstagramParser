namespace Instagram.Concrete;

internal class InstagramManager : IInstagramManager
{
    public IEnumerable<string> GetFollowers(string id)
    {
        var rnd = new Random();
        var count = rnd.Next(0, 5);
        
        for (int i = 0; i < count; i++)
        {
            yield return $"{id}_follower : {i}";
        }
    }

    public IEnumerable<string> GetFollowings(string id)
    {
        var rnd = new Random();
        var count = rnd.Next(1, 2);
        
        for (int i = 0; i < count; i++)
        {
            yield return $"{id}_following : {i}";
        }
    }
}