namespace Instagram.Concrete;

internal class UserManager : IUserManager
{
    private readonly ILamavadaClient _client;

    public UserManager(ILamavadaClient client)
    {
        _client = client;
    }

    public Task<User> GetUserById(string id, CancellationToken cancellationToken) =>
        _client.GetUserById(id, cancellationToken);

    public Task<User> GetUserByUsername(string username, CancellationToken cancellationToken) =>
        _client.GetUserByUsername(username, cancellationToken);

}