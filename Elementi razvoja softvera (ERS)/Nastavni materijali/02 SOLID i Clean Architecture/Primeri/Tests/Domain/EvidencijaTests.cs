using Domain.Enums;
using Domain.Models;
using NUnit.Framework;

namespace Tests.Domain;

[TestFixture]
public class EvidencijaTests
{
    [Test]
    [TestCase("Cao, ja sam tu.", "maja", "Maja Majic", TipEvidencije.WARNING)]
    [TestCase("Hej, ti!", "ana", "Ana Majic", TipEvidencije.ERROR)]
    [TestCase("Hej!", "marija", "Marija Majic", TipEvidencije.INFO)]
    public void Konstruktor_Ok(string opis, string korisnickoIme, string imePrezime, TipEvidencije tip)
    {
        DateTime vreme = DateTime.Now;
        Korisnik k = new(korisnickoIme, "hash", imePrezime);
        Evidencija evidencija = new(vreme, opis, k, tip);

        Assert.That(evidencija, Is.Not.Null);
        Assert.That(evidencija.Opis, Is.EqualTo(opis));
        Assert.That(evidencija.TipEvidencije, Is.EqualTo(tip));
        Assert.That(evidencija.Postavljeno, Is.EqualTo(vreme));
        Assert.That(evidencija.Autor, Is.Not.Null);
        Assert.That(evidencija.Autor.KorisnickoIme, Is.EqualTo(k.KorisnickoIme));
    }

    [Test]
    [TestCase("Cao, ja sam tu.", "maja", "Maja Majic", TipEvidencije.WARNING)]
    [TestCase("Hej, ti!", "ana", "Ana Majic", TipEvidencije.ERROR)]
    public void Property_MozeDaSeMenja(string opis, string korisnickoIme, string imePrezime, TipEvidencije tip)
    {
        DateTime vreme = DateTime.Now;
        Korisnik k = new(korisnickoIme, "hash", imePrezime);
        string noviOpis = "Opis novi";
        Evidencija evidencija = new(vreme, opis, k, tip);

        evidencija.Opis = noviOpis;

        Assert.That(evidencija.Opis, Is.EqualTo(noviOpis));
    }
}
