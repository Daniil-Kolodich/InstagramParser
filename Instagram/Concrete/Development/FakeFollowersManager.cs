namespace Instagram.Concrete.Development;

internal class FakeFollowersManager : FakeFollowManager, IFollowersManager
{
    public FakeFollowersManager() : base("followers")
    {
        
    }
}