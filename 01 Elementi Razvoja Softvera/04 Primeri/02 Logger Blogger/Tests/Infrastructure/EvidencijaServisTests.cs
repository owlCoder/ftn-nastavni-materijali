using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;
using Shared;

namespace Tests.Infrastructure;

[TestFixture]
public class EvidencijaServisTests
{
    private EvidencijaServis _evidencijaServis = null!;
    private Mock<ILoggerServis> _logger = null!;
    private Mock<IZapisiRepozitorijum> _repo = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILoggerServis>();
        _repo = new Mock<IZapisiRepozitorijum>();
        _logger.Setup(l => l.Loguj(It.IsAny<string>())).Returns(true);
        _evidencijaServis = new EvidencijaServis(_repo.Object, _logger.Object);
    }

    [Test]
    [TestCase("Program se neocekivano zatvorio", "maja", TipEvidencije.ERROR)]
    public void Objavi_ViseEvidencija_SveUspesno(string opis, string korisnickoIme, TipEvidencije tip)
    {
        Korisnik autor = new(korisnickoIme, "hash", "Maja");
        Evidencija evidencija = new(DateTime.Now, opis, autor, tip);

        _repo.Setup(r => r.Dodaj(It.IsAny<ZapisNaSajtu>())).Returns(Result<ZapisNaSajtu>.Success(evidencija));
        _repo.Setup(r => r.VratiSve())
             .Returns(Result<IReadOnlyList<ZapisNaSajtu>>.Success([evidencija, evidencija, evidencija]));

        Result<ZapisNaSajtu> a = _evidencijaServis.Objavi(evidencija);
        Result<ZapisNaSajtu> b = _evidencijaServis.Objavi(evidencija);
        Result<ZapisNaSajtu> c = _evidencijaServis.Objavi(evidencija);
        int kolicina = _evidencijaServis.PregledZapisa().Value!.Count;

        Assert.That(kolicina, Is.EqualTo(3));
        Assert.That(a.IsSuccess, Is.True);
        Assert.That(b.IsSuccess, Is.True);
        Assert.That(c.IsSuccess, Is.True);
        _repo.Verify(r => r.Dodaj(It.IsAny<ZapisNaSajtu>()), Times.Exactly(3));
        _logger.Verify(l => l.Loguj(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void Objavi_PogresanTip_VracaFailure()
    {
        Result<ZapisNaSajtu> rezultat = _evidencijaServis.Objavi(new Objava());

        Assert.That(rezultat.IsFailure, Is.True);
        _repo.Verify(r => r.Dodaj(It.IsAny<ZapisNaSajtu>()), Times.Never);
        _logger.Verify(l => l.Loguj(It.IsAny<string>()), Times.Once);
    }
}
