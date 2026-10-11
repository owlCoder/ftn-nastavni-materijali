using Application.Contracts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Domain.Models;
using Shared;

namespace Application.Services;

public sealed class AutentifikacioniServis : IAutentifikacijaServis
{
    private readonly IKorisniciRepozitorijum _korisniciRepozitorijum;
    private readonly IPasswordHasher _passwordHasher;

    public AutentifikacioniServis(IKorisniciRepozitorijum korisniciRepozitorijum, IPasswordHasher passwordHasher)
    {
        _korisniciRepozitorijum = korisniciRepozitorijum;
        _passwordHasher = passwordHasher;
    }

    public Result<Korisnik> Registracija(RegistracijaZahtev zahtev)
    {
        if (zahtev is null || string.IsNullOrWhiteSpace(zahtev.KorisnickoIme))
            return Result<Korisnik>.Failure("validacija.korisnicko_ime_obavezno", "Korisničko ime je obavezno.", new Korisnik());
        if (string.IsNullOrWhiteSpace(zahtev.Lozinka))
            return Result<Korisnik>.Failure("validacija.lozinka_obavezna", "Lozinka je obavezna.", new Korisnik());
        if (string.IsNullOrWhiteSpace(zahtev.ImePrezime))
            return Result<Korisnik>.Failure("validacija.ime_obavezno", "Ime i prezime su obavezni.", new Korisnik());

        string korisnickoIme = zahtev.KorisnickoIme.Trim();
        Result<Korisnik> postojeci = _korisniciRepozitorijum.PronadjiPoKorisnickomImenu(korisnickoIme);
        if (postojeci.IsSuccess)
            return Result<Korisnik>.Failure("auth.korisnicko_ime_zauzeto", "Korisničko ime je već zauzeto.", new Korisnik());
        if (postojeci.Error.Code != "korisnik.nije_pronadjen")
            return Result<Korisnik>.Failure(postojeci.Error.Code, postojeci.Error.Message, new Korisnik());

        Korisnik korisnik = new(korisnickoIme, _passwordHasher.Hash(zahtev.Lozinka), zahtev.ImePrezime.Trim());
        return _korisniciRepozitorijum.Dodaj(korisnik);
    }

    public Result<Korisnik> Prijava(PrijavaZahtev zahtev)
    {
        if (zahtev is null || string.IsNullOrWhiteSpace(zahtev.KorisnickoIme) || string.IsNullOrWhiteSpace(zahtev.Lozinka))
            return Result<Korisnik>.Failure("auth.neispravni_kredencijali", "Neispravni kredencijali.", new Korisnik());

        Result<Korisnik> pronadjen = _korisniciRepozitorijum.PronadjiPoKorisnickomImenu(zahtev.KorisnickoIme.Trim());
        if (pronadjen.IsFailure)
        {
            if (pronadjen.Error.Code != "korisnik.nije_pronadjen")
                return Result<Korisnik>.Failure(pronadjen.Error.Code, pronadjen.Error.Message, new Korisnik());
            return Result<Korisnik>.Failure("auth.neispravni_kredencijali", "Neispravni kredencijali.", new Korisnik());
        }

        if (!_passwordHasher.Verify(zahtev.Lozinka, pronadjen.Value.LozinkaHash))
            return Result<Korisnik>.Failure("auth.neispravni_kredencijali", "Neispravni kredencijali.", new Korisnik());

        return Result<Korisnik>.Success(pronadjen.Value);
    }
}
