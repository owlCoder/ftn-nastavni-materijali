using Domain.Enums;

namespace Domain.Models;

public sealed class Evidencija : ZapisNaSajtu
{
    public TipEvidencije TipEvidencije { get; set; }

    public Evidencija() { }

    public Evidencija(DateTime postavljeno, string opis, Korisnik autor, TipEvidencije tipEvidencije)
        : base(postavljeno, opis, autor)
    {
        TipEvidencije = tipEvidencije;
    }

    public override string ToString()
    {
        return string.Format(
            "| {0,-12} | {1,-20} | {2,-30} | {3,-20} |",
            "Log_" + TipEvidencije,
            Autor.ImePrezime,
            Opis.Length > 28 ? Opis.Substring(0, 23) + "..." : Opis,
            Postavljeno.ToString("dd.MM.yyyy HH:mm")
        ) + "\n";
    }
}
