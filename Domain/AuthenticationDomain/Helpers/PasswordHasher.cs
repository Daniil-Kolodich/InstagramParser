using System.Security.Cryptography;
using System.Text;

namespace Domain.AuthenticationDomain.Helpers;

internal static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hash = new StringBuilder();
        foreach (var b in hashedBytes) hash.Append(b.ToString("x2"));
        return hash.ToString();
    }

    public static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password).Equals(hash);
    }
}