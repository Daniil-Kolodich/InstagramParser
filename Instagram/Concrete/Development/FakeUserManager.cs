namespace Instagram.Concrete.Development;

internal class FakeUserManager : IUserManager
{
    public Task<User> GetUserById(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}