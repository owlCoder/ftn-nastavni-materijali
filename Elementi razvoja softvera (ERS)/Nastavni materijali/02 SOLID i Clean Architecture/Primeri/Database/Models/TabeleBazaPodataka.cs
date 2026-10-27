using Domain.Models;

namespace Database.Models;

public sealed class TabeleBazaPodataka
{
    public List<Korisnik> Korisnici { get; set; } = new();
    public List<Objava> Objave { get; set; } = new();
    public List<Evidencija> Evidencije { get; set; } = new();
}
