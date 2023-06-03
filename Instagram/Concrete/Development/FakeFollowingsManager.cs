namespace Instagram.Concrete.Development;

internal class FakeFollowingsManager : FakeFollowManager, IFollowingsManager
{
    public FakeFollowingsManager() : base("followings")
    {
    }
}