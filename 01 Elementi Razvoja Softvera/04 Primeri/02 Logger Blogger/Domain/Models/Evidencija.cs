using Domain.Enums;

namespace Domain.Models;

public sealed class Evidencija : ZapisNaSajtu
{
    public Evidencija() { }

    public Evidencija(DateTime postavljeno, string opis, Korisnik autor, TipEvidencije tipEvidencije) : base(postavljeno, opis, autor)
    {
        TipEvidencije = tipEvidencije;
    }

    public TipEvidencije TipEvidencije { get; set; }
}
