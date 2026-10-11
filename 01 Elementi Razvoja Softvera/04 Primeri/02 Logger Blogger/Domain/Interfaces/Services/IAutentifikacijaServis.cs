using Domain.Contracts;
using Domain.Models;
using Shared;

namespace Domain.Interfaces.Services;

public interface IAutentifikacijaServis
{
    /// <summary>Registruje novog korisnika; lozinka se hešira pre čuvanja.</summary>
    Result<Korisnik> Registracija(RegistracijaZahtev zahtev);

    /// <summary>Prijavljuje korisnika na osnovu korisničkog imena i lozinke.</summary>
    Result<Korisnik> Prijava(PrijavaZahtev zahtev);
}
