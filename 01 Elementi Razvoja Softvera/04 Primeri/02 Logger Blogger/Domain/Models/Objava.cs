using Domain.Enums;

namespace Domain.Models;

public sealed class Objava : ZapisNaSajtu
{
    public Objava() { }

    public Objava(DateTime postavljeno, string opis, Korisnik autor, string naslov, TipObjave tipObjave) : base(postavljeno, opis, autor)
    {
        Naslov = naslov;
        TipObjave = tipObjave;
    }

    public string Naslov { get; set; } = string.Empty;
    public TipObjave TipObjave { get; set; }
}
