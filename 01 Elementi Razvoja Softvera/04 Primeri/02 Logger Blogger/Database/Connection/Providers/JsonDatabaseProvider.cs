using Domain.Interfaces.Database;
using Shared;

namespace Database.Connection.Providers;

public sealed class JsonDatabaseProvider : IDatabaseProvider
{
    public string Naziv => "json";

    public IBazaPodataka BazaPodataka { get; } = new JsonBazaPodataka();

    public Result<Unit> PokusajKonekciju() => Result<Unit>.Success(new Unit());
}
