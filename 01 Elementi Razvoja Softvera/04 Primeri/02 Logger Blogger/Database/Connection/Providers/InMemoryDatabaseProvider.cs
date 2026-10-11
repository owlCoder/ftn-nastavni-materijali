using Database.Interfaces;
using Shared;

namespace Database.Connection.Providers;

public sealed class InMemoryDatabaseProvider : IDatabaseProvider
{
    public string Naziv => "in-memory";

    public IBazaPodataka BazaPodataka { get; } = new InMemoryBazaPodataka();

    public Result<Unit> PokusajKonekciju() => Result<Unit>.Success(new Unit());
}
