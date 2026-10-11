using Application.Contracts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Domain.Models;
using Application.Services;
using Moq;
using NUnit.Framework;
using Shared;

namespace Tests.Application;

[TestFixture]
public class AutentifikacioniServisTests
{
    private Mock<IKorisniciRepozitorijum> _repo = null!;
    private Mock<IPasswordHasher> _hasher = null!;
    private AutentifikacioniServis _servis = null!;

    [SetUp]
    public void Setup()
    {
        _repo = new Mock<IKorisniciRepozitorijum>();
        _hasher = new Mock<IPasswordHasher>();
        _servis = new AutentifikacioniServis(_repo.Object, _hasher.Object);
    }

    [Test]
    public void Prijava_IspravniPodaci_VracaSuccess()
    {
        Korisnik korisnik = new("maja", "HASH", "Maja Taron");
        _repo.Setup(r => r.PronadjiPoKorisnickomImenu("maja")).Returns(Result<Korisnik>.Success(korisnik));
        _hasher.Setup(h => h.Verify("123", "HASH")).Returns(true);

        Result<Korisnik> rezultat = _servis.Prijava(new PrijavaZahtev("maja", "123"));

        Assert.That(rezultat.IsSuccess, Is.True);
        Assert.That(rezultat.Value, Is.Not.Null);
        Assert.That(rezultat.Value!.KorisnickoIme, Is.EqualTo("maja"));
    }

    [Test]
    public void Prijava_PogresnaLozinka_VracaFailure()
    {
        Korisnik korisnik = new("maja", "HASH", "Maja Taron");
        _repo.Setup(r => r.PronadjiPoKorisnickomImenu("maja")).Returns(Result<Korisnik>.Success(korisnik));
        _hasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        Result<Korisnik> rezultat = _servis.Prijava(new PrijavaZahtev("maja", "pogresno"));

        Assert.That(rezultat.IsFailure, Is.True);
        Assert.That(rezultat.Error.Code, Is.EqualTo("auth.neispravni_kredencijali"));
    }

    [Test]
    public void Prijava_NepostojeciKorisnik_VracaFailure()
    {
        _repo.Setup(r => r.PronadjiPoKorisnickomImenu(It.IsAny<string>()))
             .Returns(Result<Korisnik>.Failure("korisnik.nije_pronadjen", "Korisnik nije pronađen.", new Korisnik()));

        Result<Korisnik> rezultat = _servis.Prijava(new PrijavaZahtev("nepostoji", "123"));

        Assert.That(rezultat.IsFailure, Is.True);
        Assert.That(rezultat.Error.Code, Is.EqualTo("auth.neispravni_kredencijali"));
    }

    [Test]
    public void Registracija_NovKorisnik_HesiraLozinkuIDodaje()
    {
        _repo.Setup(r => r.PronadjiPoKorisnickomImenu("nova"))
             .Returns(Result<Korisnik>.Failure("korisnik.nije_pronadjen", "Korisnik nije pronađen.", new Korisnik()));
        _hasher.Setup(h => h.Hash("tajna")).Returns("HASHED");
        _repo.Setup(r => r.Dodaj(It.IsAny<Korisnik>()))
             .Returns<Korisnik>(Result<Korisnik>.Success);

        Result<Korisnik> rezultat = _servis.Registracija(new RegistracijaZahtev("nova", "tajna", "Nova Korisnica"));

        Assert.That(rezultat.IsSuccess, Is.True);
        Assert.That(rezultat.Value!.LozinkaHash, Is.EqualTo("HASHED"));
        _repo.Verify(r => r.Dodaj(It.Is<Korisnik>(k => k.LozinkaHash == "HASHED")), Times.Once);
    }

    [Test]
    public void Registracija_ZauzetoKorisnickoIme_VracaFailure()
    {
        _repo.Setup(r => r.PronadjiPoKorisnickomImenu("maja"))
             .Returns(Result<Korisnik>.Success(new Korisnik("maja", "HASH", "Maja")));

        Result<Korisnik> rezultat = _servis.Registracija(new RegistracijaZahtev("maja", "123", "Maja"));

        Assert.That(rezultat.IsFailure, Is.True);
        Assert.That(rezultat.Error.Code, Is.EqualTo("auth.korisnicko_ime_zauzeto"));
        _repo.Verify(r => r.Dodaj(It.IsAny<Korisnik>()), Times.Never);
    }

    [Test]
    public void Registracija_PraznaLozinka_VracaFailure()
    {
        Result<Korisnik> rezultat = _servis.Registracija(new RegistracijaZahtev("maja", "  ", "Maja"));

        Assert.That(rezultat.IsFailure, Is.True);
        Assert.That(rezultat.Error.Code, Is.EqualTo("validacija.lozinka_obavezna"));
    }
}
