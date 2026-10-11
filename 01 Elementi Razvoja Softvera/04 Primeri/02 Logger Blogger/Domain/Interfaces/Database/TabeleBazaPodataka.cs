using Domain.Models;

namespace Domain.Interfaces.Database;

/// <summary>In-memory predstava "tabela" skladišta podataka.</summary>
public sealed class TabeleBazaPodataka
{
    public List<Korisnik> Korisnici { get; set; } = [];
    public List<Objava> Objave { get; set; } = [];
    public List<Evidencija> Evidencije { get; set; } = [];
}
