using Database.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Models;
using Shared;

namespace Database.Repositories;

public sealed class KorisniciRepozitorijum : IKorisniciRepozitorijum
{
    private readonly IBazaPodataka _bazaPodataka;

    public KorisniciRepozitorijum(IBazaPodataka bazaPodataka)
    {
        _bazaPodataka = bazaPodataka;
    }

    public Result<Korisnik> Dodaj(Korisnik korisnik)
    {
        if (!_bazaPodataka.IsReady)
            return Result<Korisnik>.Failure("baza.nedostupna", "Skladište nije spremno.", new Korisnik());

        bool postoji = _bazaPodataka.Tabele.Korisnici.Any(
            k => string.Equals(k.KorisnickoIme, korisnik.KorisnickoIme, StringComparison.OrdinalIgnoreCase));
        if (postoji)
        {
            return Result<Korisnik>.Failure("korisnik.duplikat", "Korisnik sa istim korisničkim imenom već postoji.", new Korisnik());
        }

        _bazaPodataka.Tabele.Korisnici.Add(korisnik);
        if (!_bazaPodataka.SacuvajPromene())
        {
            _bazaPodataka.Tabele.Korisnici.Remove(korisnik);
            return Result<Korisnik>.Failure("baza.upis", "Upis nije uspeo.", new Korisnik());
        }
        return Result<Korisnik>.Success(korisnik);
    }

    public Result<Korisnik> PronadjiPoKorisnickomImenu(string korisnickoIme)
    {
        if (!_bazaPodataka.IsReady)
            return Result<Korisnik>.Failure("baza.nedostupna", "Skladište nije spremno.", new Korisnik());

        Korisnik? korisnik = _bazaPodataka.Tabele.Korisnici.FirstOrDefault(
            k => string.Equals(k.KorisnickoIme, korisnickoIme, StringComparison.OrdinalIgnoreCase));
        if (korisnik is null)
        {
            return Result<Korisnik>.Failure("korisnik.nije_pronadjen", "Korisnik nije pronađen.", new Korisnik());
        }

        return Result<Korisnik>.Success(korisnik);
    }

    public Result<IReadOnlyList<Korisnik>> VratiSve()
    {
        if (!_bazaPodataka.IsReady)
            return Result<IReadOnlyList<Korisnik>>.Failure("baza.nedostupna", "Skladište nije spremno.", new List<Korisnik>());

        return Result<IReadOnlyList<Korisnik>>.Success(_bazaPodataka.Tabele.Korisnici.ToList());
    }
}
