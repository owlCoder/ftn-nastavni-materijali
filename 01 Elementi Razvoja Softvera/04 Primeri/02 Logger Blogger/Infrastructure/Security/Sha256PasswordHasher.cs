using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces.Security;

namespace Infrastructure.Security;

/// <summary>
/// Konkretna implementacija heširanja preko SHA-256. Jedina klasa koja zna za algoritam,
/// pa zamena (npr. PBKDF2/BCrypt) ne dira ostatak aplikacije (OCP).
/// </summary>
public sealed class Sha256PasswordHasher : IPasswordHasher
{
    public string Hash(string plainPassword)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainPassword));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string plainPassword, string passwordHash)
    {
        string computed = Hash(plainPassword);
        return string.Equals(computed, passwordHash, StringComparison.Ordinal);
    }
}
