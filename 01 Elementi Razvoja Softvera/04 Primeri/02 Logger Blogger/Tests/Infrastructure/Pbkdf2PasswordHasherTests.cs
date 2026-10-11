using Infrastructure.Security;
using NUnit.Framework;

namespace Tests.Infrastructure;

[TestFixture]
public sealed class Pbkdf2PasswordHasherTests
{
    [Test]
    public void Hash_SamePassword_UsesDifferentSalts()
    {
        Pbkdf2PasswordHasher hasher = new();
        string first = hasher.Hash("nastavnaLozinka123");
        string second = hasher.Hash("nastavnaLozinka123");

        Assert.That(first, Is.Not.EqualTo(second));
        Assert.That(hasher.Verify("nastavnaLozinka123", first), Is.True);
        Assert.That(hasher.Verify("pogresna", first), Is.False);
        Assert.That(hasher.Verify("nastavnaLozinka123", second), Is.True);
    }

    [Test]
    public void Verify_InvalidHash_DoesNotAuthenticate()
    {
        Pbkdf2PasswordHasher hasher = new();
        Assert.That(hasher.Verify("test", "neispravan$format"), Is.False);
        Assert.That(hasher.Verify("test", "pbkdf2-sha256$310000$!$!"), Is.False);
    }
}
