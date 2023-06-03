using Instagram.Concrete;

namespace Instagram;

public interface IFollowManager
{
    /// <param name="query"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <param name="userId"></param>
    /// <summary>
    /// Search Children
    /// </summary>
    /// <remarks>
    /// Search users by child users
    /// </remarks>
    /// <returns>Successful Response</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<ICollection<UserShort>> Search(string userId, string query,
        CancellationToken cancellationToken);

    /// <param name="amount"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <param name="userId"></param>
    /// <summary>
    /// User Children
    /// </summary>
    /// <remarks>
    /// Get child users
    /// </remarks>
    /// <returns>Successful Response</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<IEnumerable<ICollection<UserShort>>> Get(string userId, int? amount,
        CancellationToken cancellationToken);
}