namespace Instagram;

public interface IInstagramManager
{
    IEnumerable<string> GetFollowers(string id);
    IEnumerable<string> GetFollowings(string id);
}