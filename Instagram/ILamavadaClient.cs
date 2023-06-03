using Instagram.Concrete;

namespace Instagram;

internal interface ILamavadaClient
{
    Task<User> GetUserById(string id, CancellationToken cancellationToken);
    Task<User> GetUserByUsername(string username, CancellationToken cancellationToken);
    Task<ICollection<UserShort>> SearchFollowers(string userId, string query, CancellationToken cancellationToken);
    Task<ICollection<UserShort>> GetFollowers(string userId, int? amount, CancellationToken cancellationToken);
    Task<Tuple<ICollection<UserShort>, string>> GetFollowersChunk(string userId, int? amount, string maxId,
        CancellationToken cancellationToken);
    
    Task<ICollection<UserShort>> SearchFollowings(string userId, string query, CancellationToken cancellationToken);
    Task<ICollection<UserShort>> GetFollowings(string userId, int? amount, CancellationToken cancellationToken);
    Task<Tuple<ICollection<UserShort>, string>> GetFollowingsChunk(string userId, int? amount, string maxId,
        CancellationToken cancellationToken);
}