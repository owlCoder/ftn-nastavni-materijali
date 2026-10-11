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
public class BlogServisTests
{
    private BlogServis _blogServis = null!;
    private Mock<ILoggerServis> _logger = null!;
    private Mock<IZapisiRepozitorijum> _repo = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILoggerServis>();
        _repo = new Mock<IZapisiRepozitorijum>();

        _repo.Setup(r => r.Dodaj(It.IsAny<ZapisNaSajtu>()))
             .Returns<ZapisNaSajtu>(Result<ZapisNaSajtu>.Success);
        _logger.Setup(l => l.Loguj(It.IsAny<string>())).Returns(true);

        _blogServis = new BlogServis(_repo.Object, _logger.Object);
    }

    [Test]
    [TestCase("Opis objave", "maja", "Objava 1", TipObjave.FRIENDS)]
    public void Objavi_ValidnaObjava_VracaSuccess(string opis, string korisnickoIme, string naslov, TipObjave tip)
    {
        Korisnik autor = new(korisnickoIme, "hash", "Maja");
        Objava objava = new(DateTime.Now, opis, autor, naslov, tip);

        Result<ZapisNaSajtu> rezultat = _blogServis.Objavi(objava);

        Assert.That(rezultat.IsSuccess, Is.True);
        _repo.Verify(r => r.Dodaj(objava), Times.Once);
        _logger.Verify(l => l.Loguj(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Test]
    public void Objavi_PogresanTip_VracaFailure()
    {
        Evidencija evidencija = new();

        Result<ZapisNaSajtu> rezultat = _blogServis.Objavi(evidencija);

        Assert.That(rezultat.IsFailure, Is.True);
        _repo.Verify(r => r.Dodaj(It.IsAny<ZapisNaSajtu>()), Times.Never);
        _logger.Verify(l => l.Loguj(It.IsAny<string>()), Times.Once);
    }
}
