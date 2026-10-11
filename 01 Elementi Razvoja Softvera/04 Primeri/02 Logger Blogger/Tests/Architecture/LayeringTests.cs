using Application.Services;
using Database.Repositories;
using Domain.Models;
using Infrastructure.Security;
using NUnit.Framework;

namespace Tests.Architecture;

[TestFixture]
public sealed class LayeringTests
{
    [Test]
    public void Domain_NemaProjektneZavisnosti()
    {
        var local = new[] { "Application", "Database", "Infrastructure", "Presentation" };
        var referenced = typeof(Korisnik).Assembly.GetReferencedAssemblies().Select(x => x.Name).ToArray();
        Assert.That(referenced.Intersect(local), Is.Empty);
    }

    [Test]
    public void Application_NeZavisiOdInfrastruktureIPrezentacije()
    {
        var forbidden = new[] { "Database", "Infrastructure", "Presentation" };
        var referenced = typeof(AutentifikacioniServis).Assembly.GetReferencedAssemblies().Select(x => x.Name).ToArray();
        Assert.That(referenced.Intersect(forbidden), Is.Empty);
    }

    [Test]
    public void Projekti_ImajuRazdvojeneSlojeve()
    {
        Assert.That(typeof(Korisnik).Assembly, Is.Not.EqualTo(typeof(AutentifikacioniServis).Assembly));
        Assert.That(typeof(AutentifikacioniServis).Assembly, Is.Not.EqualTo(typeof(KorisniciRepozitorijum).Assembly));
        Assert.That(typeof(Pbkdf2PasswordHasher).Assembly, Is.Not.EqualTo(typeof(AutentifikacioniServis).Assembly));
    }
}
