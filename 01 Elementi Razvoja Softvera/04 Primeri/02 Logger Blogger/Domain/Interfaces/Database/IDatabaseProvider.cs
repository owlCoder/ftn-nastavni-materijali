using Shared;

namespace Domain.Interfaces.Database;

/// <summary>
/// Provajder skladišta podataka: zna svoje ime i ume da proveri konekciju.
/// Bira se na jednom mestu (factory) umesto da se "pogađa" tokom pokretanja.
/// </summary>
public interface IDatabaseProvider
{
    string Naziv { get; }

    IBazaPodataka BazaPodataka { get; }

    Result<Unit> PokusajKonekciju();
}
