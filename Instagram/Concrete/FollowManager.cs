namespace Instagram.Concrete;

internal abstract class FollowManager : IFollowManager
{
    private readonly Func<string, string, CancellationToken, Task<ICollection<UserShort>>> _searchDelegate;
    private readonly Func<string, int?, CancellationToken, Task<ICollection<UserShort>>> _getDelegate;
    private readonly Func<string, int?, string, CancellationToken, Task<Tuple<ICollection<UserShort>, string>>> _getChunkDelegate;

    public FollowManager(Func<string, string, CancellationToken, Task<ICollection<UserShort>>> searchDelegate,
        Func<string, int?, CancellationToken, Task<ICollection<UserShort>>> getDelegate,
        Func<string, int?, string, CancellationToken, Task<Tuple<ICollection<UserShort>, string>>> getChunkDelegate)
    {
        _searchDelegate = searchDelegate;
        _getDelegate = getDelegate;
        _getChunkDelegate = getChunkDelegate;
    }

    public Task<ICollection<UserShort>> Search(string userId, string query, CancellationToken cancellationToken) =>
        _searchDelegate(userId, query, cancellationToken);

    public Task<ICollection<UserShort>> Get(string userId, int? amount, CancellationToken cancellationToken) =>
        _getDelegate(userId, amount, cancellationToken);

    public Task<Tuple<ICollection<UserShort>, string>> GetChunk(string userId, int? amount, string maxId,
        CancellationToken cancellationToken) =>
        _getChunkDelegate(userId, amount, maxId, cancellationToken);
}