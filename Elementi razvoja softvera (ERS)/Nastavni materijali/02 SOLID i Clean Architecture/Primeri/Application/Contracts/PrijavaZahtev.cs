namespace Application.Contracts;

public sealed class PrijavaZahtev
{
    public PrijavaZahtev() { }

    public PrijavaZahtev(string korisnickoIme, string lozinka)
    {
        KorisnickoIme = korisnickoIme;
        Lozinka = lozinka;
    }

    public string KorisnickoIme { get; set; } = string.Empty;
    public string Lozinka { get; set; } = string.Empty;
}
