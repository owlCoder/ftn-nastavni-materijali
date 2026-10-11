using System.Globalization;

namespace Domain.Models;

/// <summary>
/// Zajednička osnova za svaki zapis na sajtu (objava ili evidencija). Drži autora,
/// opis i vreme postavljanja. Konkretni tipovi dodaju svoja polja i prikaz.
/// </summary>
public abstract class ZapisNaSajtu
{
    public DateTime Postavljeno { get; set; }
    public string Opis { get; set; } = string.Empty;
    public Korisnik Autor { get; set; } = new();

    protected ZapisNaSajtu() { }

    protected ZapisNaSajtu(DateTime postavljeno, string opis, Korisnik autor)
    {
        Postavljeno = postavljeno;
        Opis = opis;
        Autor = autor;
    }

    public override string ToString()
    {
        // Svaki zapis se prikazuje kao jedan red tabele.
        return string.Format(
            CultureInfo.InvariantCulture,
            "| {0,-20} | {1,-30} | {2,-20} |",
            Autor.ImePrezime,
            Opis.Length > 28 ? Opis.Substring(0, 28) + "..." : Opis,
            Postavljeno.ToString("dd.MM.yyyy HH:mm")
        );
    }
}
