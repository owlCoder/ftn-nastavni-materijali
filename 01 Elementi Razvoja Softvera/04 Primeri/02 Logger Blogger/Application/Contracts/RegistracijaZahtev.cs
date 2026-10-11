namespace Application.Contracts;

public sealed class RegistracijaZahtev
{
    public RegistracijaZahtev() { }

    public RegistracijaZahtev(string korisnickoIme, string lozinka, string imePrezime)
    {
        KorisnickoIme = korisnickoIme;
        Lozinka = lozinka;
        ImePrezime = imePrezime;
    }

    public string KorisnickoIme { get; set; } = string.Empty;
    public string Lozinka { get; set; } = string.Empty;
    public string ImePrezime { get; set; } = string.Empty;
}
