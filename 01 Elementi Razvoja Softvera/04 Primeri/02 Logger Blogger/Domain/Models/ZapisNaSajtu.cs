namespace Domain.Models;

public abstract class ZapisNaSajtu
{
    protected ZapisNaSajtu() { }

    protected ZapisNaSajtu(DateTime postavljeno, string opis, Korisnik autor)
    {
        Postavljeno = postavljeno;
        Opis = opis;
        Autor = autor;
    }

    public DateTime Postavljeno { get; set; }
    public string Opis { get; set; } = string.Empty;
    public Korisnik Autor { get; set; } = new();
}
