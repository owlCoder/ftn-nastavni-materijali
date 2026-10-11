using Shared;

namespace Database.Interfaces;

public interface IDatabaseProvider
{
    string Naziv { get; }
    IBazaPodataka BazaPodataka { get; }
    Result<Unit> PokusajKonekciju();
}
