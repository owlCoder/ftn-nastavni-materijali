using Domain.Models;
using Shared;

namespace Application.Interfaces.Repositories;

public interface IKorisniciRepozitorijum
{
    /// <summary>Dodaje korisnika u bazu ako korisničko ime već ne postoji.</summary>
    Result<Korisnik> Dodaj(Korisnik korisnik);

    /// <summary>Pronalazi korisnika po korisničkom imenu.</summary>
    Result<Korisnik> PronadjiPoKorisnickomImenu(string korisnickoIme);

    /// <summary>Vraća sve korisnike.</summary>
    Result<IReadOnlyList<Korisnik>> VratiSve();
}
