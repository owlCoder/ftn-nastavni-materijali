using Application.Contracts;
using Domain.Models;
using Shared;

namespace Application.Interfaces.Services;

public interface IAutentifikacijaServis
{
    /// <summary>Registruje novog korisnika; lozinka se hešira pre čuvanja.</summary>
    Result<Korisnik> Registracija(RegistracijaZahtev zahtev);

    /// <summary>Prijavljuje korisnika na osnovu korisničkog imena i lozinke.</summary>
    Result<Korisnik> Prijava(PrijavaZahtev zahtev);
}
