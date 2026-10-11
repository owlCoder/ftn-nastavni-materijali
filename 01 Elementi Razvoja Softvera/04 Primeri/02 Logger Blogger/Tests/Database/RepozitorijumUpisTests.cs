using Application.Interfaces.Repositories;
using Database.Interfaces;
using Database.Models;
using Database.Repositories;
using Domain.Models;
using Moq;
using NUnit.Framework;

namespace Tests.Database;

[TestFixture]
public sealed class RepozitorijumUpisTests
{
    [Test]
    public void Dodaj_NeuspesanUpis_VracaGreskuIVracaStanje()
    {
        TabeleBazaPodataka tabele = new();
        Mock<IBazaPodataka> baza = new();
        baza.SetupGet(x => x.IsReady).Returns(true);
        baza.SetupGet(x => x.Tabele).Returns(tabele);
        baza.Setup(x => x.SacuvajPromene()).Returns(false);

        IKorisniciRepozitorijum repo = new KorisniciRepozitorijum(baza.Object);
        var rezultat = repo.Dodaj(new Korisnik("maja", "hash", "Maja"));

        Assert.That(rezultat.IsFailure, Is.True);
        Assert.That(rezultat.Error.Code, Is.EqualTo("baza.upis"));
        Assert.That(rezultat.Value.KorisnickoIme, Is.Empty);
        Assert.That(tabele.Korisnici, Is.Empty);
    }

    [Test]
    public void Pretraga_NedostupnaBaza_NijeIstoStoINepostojeciKorisnik()
    {
        Mock<IBazaPodataka> baza = new();
        baza.SetupGet(x => x.IsReady).Returns(false);
        IKorisniciRepozitorijum repo = new KorisniciRepozitorijum(baza.Object);

        var rezultat = repo.PronadjiPoKorisnickomImenu("maja");

        Assert.That(rezultat.Error.Code, Is.EqualTo("baza.nedostupna"));
    }
}
