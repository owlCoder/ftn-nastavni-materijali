namespace Domain.Models;

/// <summary>
/// Korisnik sistema. Lozinka se nikada ne čuva u čistom obliku — drži se samo heš
/// (<see cref="LozinkaHash"/>), koji popunjava <c>IPasswordHasher</c> u sloju infrastrukture.
/// </summary>
public sealed class Korisnik
{
    public string KorisnickoIme { get; set; } = string.Empty;
    public string LozinkaHash { get; set; } = string.Empty;
    public string ImePrezime { get; set; } = string.Empty;

    public Korisnik() { }

    public Korisnik(string korisnickoIme, string lozinkaHash, string imePrezime)
    {
        KorisnickoIme = korisnickoIme;
        LozinkaHash = lozinkaHash;
        ImePrezime = imePrezime;
    }
}
