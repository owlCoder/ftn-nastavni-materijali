using Domain.Enums;

namespace Domain.Models;

public sealed class Objava : ZapisNaSajtu
{
    public string Naslov { get; set; } = string.Empty;
    public TipObjave TipObjave { get; set; }

    public Objava() { }

    public Objava(DateTime postavljeno, string opis, Korisnik autor, string naslov, TipObjave tipObjave)
        : base(postavljeno, opis, autor)
    {
        Naslov = naslov;
        TipObjave = tipObjave;
    }

    public override string ToString()
    {
        return string.Format(
            "| {0,-8} | {1,-15} | {2,-20} | {3,-30} | {4,-20} |",
            "#" + TipObjave,
            Naslov.Length > 13 ? Naslov.Substring(0, 13) + "..." : Naslov,
            Autor.ImePrezime,
            Opis.Length > 28 ? Opis.Substring(0, 23) + "..." : Opis,
            Postavljeno.ToString("dd.MM.yyyy HH:mm")
        ) + "\n";
    }
}
