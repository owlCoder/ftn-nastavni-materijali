using Domain.Interfaces.Database;
using Domain.Interfaces.Repositories;
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
        bool postoji = _bazaPodataka.Tabele.Korisnici.Any(
            k => string.Equals(k.KorisnickoIme, korisnik.KorisnickoIme, StringComparison.OrdinalIgnoreCase));
        if (postoji)
        {
            return Result<Korisnik>.Failure("korisnik.duplikat", "Korisnik sa istim korisničkim imenom već postoji.");
        }

        _bazaPodataka.Tabele.Korisnici.Add(korisnik);
        _bazaPodataka.SacuvajPromene();
        return Result<Korisnik>.Success(korisnik);
    }

    public Result<Korisnik> PronadjiPoKorisnickomImenu(string korisnickoIme)
    {
        Korisnik? korisnik = _bazaPodataka.Tabele.Korisnici.FirstOrDefault(
            k => string.Equals(k.KorisnickoIme, korisnickoIme, StringComparison.OrdinalIgnoreCase));
        if (korisnik is null)
        {
            return Result<Korisnik>.Failure("korisnik.nije_pronadjen", "Korisnik nije pronađen.");
        }

        return Result<Korisnik>.Success(korisnik);
    }

    public Result<IReadOnlyList<Korisnik>> VratiSve()
    {
        return Result<IReadOnlyList<Korisnik>>.Success(_bazaPodataka.Tabele.Korisnici.ToList());
    }
}
