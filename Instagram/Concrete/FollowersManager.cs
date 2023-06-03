namespace Instagram.Concrete;

internal class FollowersManager : FollowManager, IFollowersManager 
{
    public FollowersManager(ILamavadaClient client) : 
        base(client.SearchFollowers, client.GetFollowers, client.GetFollowersChunk)
    { }
}