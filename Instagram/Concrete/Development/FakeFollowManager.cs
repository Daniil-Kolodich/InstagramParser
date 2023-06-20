namespace Instagram.Concrete.Development;

internal abstract class FakeFollowManager : IFollowManager
{
    private readonly string _prefix;

    protected FakeFollowManager(string prefix)
    {
        _prefix = prefix;
    }

    public Task<ICollection<UserShort>> Search(string userId, string query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ICollection<UserShort>>> Get(string userId, int? amount, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Tuple<ICollection<UserShort>, string>> GetChunk(string userId, int? amount, string maxId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static UserShort CreateUserShort(string pk, string userName)
    {
        return new UserShort
        {
            Pk = pk,
            Username = userName,
            FullName = "",
            Stories = new object[] {},
            AdditionalProperties = new Dictionary<string, object>(),
            IsPrivate = false,
            IsVerified = false,
            ProfilePicUrl = new Uri(""),
            ProfilePicUrlHd = new Uri(""),
        };
    }
}