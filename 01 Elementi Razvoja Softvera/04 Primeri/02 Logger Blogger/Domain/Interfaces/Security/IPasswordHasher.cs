namespace Domain.Interfaces.Security;

/// <summary>
/// Apstrakcija nad heširanjem lozinki. Domen i servisi zavise od ovog ugovora,
/// a ne od konkretnog algoritma — konkretna implementacija živi u infrastrukturi (DIP).
/// </summary>
public interface IPasswordHasher
{
    string Hash(string plainPassword);

    bool Verify(string plainPassword, string passwordHash);
}
