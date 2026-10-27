using Domain.Enums;
using Domain.Models;
using NUnit.Framework;

namespace Tests.Domain;

[TestFixture]
public class ObjavaTests
{
    [Test]
    [TestCase("Cao, ja sam tu.", "maja", "Maja Majic", "Naslov: Caos", TipObjave.FRIENDS)]
    [TestCase("Hej, ti!", "ana", "Ana Majic", "Naslov: Hej", TipObjave.VLOG)]
    [TestCase("Hej!", "marija", "Marija Majic", "Naslov: Hej Hej", TipObjave.FAMILY)]
    public void Konstruktor_Ok(string opis, string korisnickoIme, string imePrezime, string naslov, TipObjave tip)
    {
        DateTime vreme = DateTime.Now;
        Korisnik k = new(korisnickoIme, "hash", imePrezime);
        Objava objava = new(vreme, opis, k, naslov, tip);

        Assert.That(objava, Is.Not.Null);
        Assert.That(objava.Opis, Is.EqualTo(opis));
        Assert.That(objava.Naslov, Is.EqualTo(naslov));
        Assert.That(objava.TipObjave, Is.EqualTo(tip));
        Assert.That(objava.Postavljeno, Is.EqualTo(vreme));
        Assert.That(objava.Autor, Is.Not.Null);
        Assert.That(objava.Autor.KorisnickoIme, Is.EqualTo(k.KorisnickoIme));
    }
}
