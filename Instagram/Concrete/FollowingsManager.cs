namespace Instagram.Concrete;

internal class FollowingsManager : FollowManager, IFollowingsManager 
{
    public FollowingsManager(ILamavadaClient client) : 
        base(client.SearchFollowings, client.GetFollowings, client.GetFollowingsChunk)
    { }
}