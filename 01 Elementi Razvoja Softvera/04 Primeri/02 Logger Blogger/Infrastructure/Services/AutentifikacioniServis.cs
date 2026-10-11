using Domain.Contracts;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using Domain.Interfaces.Services;
using Domain.Models;
using Shared;

namespace Infrastructure.Services;

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
        if (string.IsNullOrWhiteSpace(zahtev.KorisnickoIme))
        {
            return Result<Korisnik>.Failure("validacija.korisnicko_ime_obavezno", "Korisničko ime je obavezno.");
        }

        if (string.IsNullOrWhiteSpace(zahtev.Lozinka))
        {
            return Result<Korisnik>.Failure("validacija.lozinka_obavezna", "Lozinka je obavezna.");
        }

        Result<Korisnik> postojeci = _korisniciRepozitorijum.PronadjiPoKorisnickomImenu(zahtev.KorisnickoIme);
        if (postojeci.IsSuccess)
        {
            return Result<Korisnik>.Failure("auth.korisnicko_ime_zauzeto", "Korisničko ime je već zauzeto.");
        }

        Korisnik korisnik = new(
            zahtev.KorisnickoIme.Trim(),
            _passwordHasher.Hash(zahtev.Lozinka),
            zahtev.ImePrezime.Trim()
        );

        return _korisniciRepozitorijum.Dodaj(korisnik);
    }

    public Result<Korisnik> Prijava(PrijavaZahtev zahtev)
    {
        Result<Korisnik> korisnikResult = _korisniciRepozitorijum.PronadjiPoKorisnickomImenu(zahtev.KorisnickoIme);
        if (korisnikResult.IsFailure || korisnikResult.Value is null)
        {
            return Result<Korisnik>.Failure("auth.neispravni_kredencijali", "Neispravni kredencijali.");
        }

        if (!_passwordHasher.Verify(zahtev.Lozinka, korisnikResult.Value.LozinkaHash))
        {
            return Result<Korisnik>.Failure("auth.neispravni_kredencijali", "Neispravni kredencijali.");
        }

        return Result<Korisnik>.Success(korisnikResult.Value);
    }
}
