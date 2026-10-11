using Domain.Models;
using NUnit.Framework;

namespace Tests.Domain;

[TestFixture]
public class KorisnikTests
{
    [Test]
    [TestCase("bojana", "hash123", "Bojana Andic")]
    [TestCase("ana", "hash123", "Ana Lanic")]
    [TestCase("marko", "hash123", "Marko Lukic")]
    public void Konstruktor_Ok(string korisnickoIme, string lozinkaHash, string imePrezime)
    {
        Korisnik korisnik = new(korisnickoIme, lozinkaHash, imePrezime);

        Assert.That(korisnik, Is.Not.Null);
        Assert.That(korisnik.KorisnickoIme, Is.EqualTo(korisnickoIme));
        Assert.That(korisnik.LozinkaHash, Is.EqualTo(lozinkaHash));
        Assert.That(korisnik.ImePrezime, Is.EqualTo(imePrezime));
    }
}
