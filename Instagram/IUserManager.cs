using Instagram.Concrete;

namespace Instagram;

public interface IUserManager
{
    /// <param name="id"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// User By Id
    /// </summary>
    /// <remarks>
    /// Get user object by id
    /// </remarks>
    /// <returns>Successful Response</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<User> GetUserById(string id, CancellationToken cancellationToken);
    
    /// <param name="username"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get user object by username (one request required)
    /// </summary>
    /// <remarks>
    /// Get user object by username
    /// </remarks>
    /// <returns>Successful Response</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    Task<User> GetUserByUsername(string username, CancellationToken cancellationToken);
}