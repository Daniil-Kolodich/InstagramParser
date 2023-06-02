using Instagram.Concrete;

namespace Instagram;

public interface IFollowingsManager : IFollowManager
{
    /// <param name="query"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <param name="userId"></param>
    /// <summary>
    /// Search Followings
    /// </summary>
    /// <remarks>
    /// Search users by following users
    /// </remarks>
    /// <returns>Successful Response</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    new Task<ICollection<UserShort>> Search(string userId, string query,
        CancellationToken cancellationToken);

    /// <param name="amount"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <param name="userId"></param>
    /// <summary>
    /// User Followings
    /// </summary>
    /// <remarks>
    /// Get following users
    /// </remarks>
    /// <returns>Successful Response</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    new Task<ICollection<UserShort>> Get(string userId, int? amount,
        CancellationToken cancellationToken);
    
    /// <param name="maxId"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <param name="userId"></param>
    /// <param name="amount"></param>
    /// <summary>
    /// Get a user followings (one request required)
    /// </summary>
    /// <remarks>
    /// Get part (one page) of following users with cursor
    /// </remarks>
    /// <returns>List of users (followings)</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    new Task<Tuple<ICollection<UserShort>, string>> GetChunk(string userId, int? amount,
        string maxId, CancellationToken cancellationToken);
}