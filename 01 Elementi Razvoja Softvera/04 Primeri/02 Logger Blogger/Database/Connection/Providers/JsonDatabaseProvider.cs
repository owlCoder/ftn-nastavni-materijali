using Database.Interfaces;
using Shared;

namespace Database.Connection.Providers;

public sealed class JsonDatabaseProvider : IDatabaseProvider
{
    public JsonDatabaseProvider(string putanja = "podaci.json")
    {
        BazaPodataka = new JsonBazaPodataka(putanja);
    }

    public string Naziv => "json";
    public IBazaPodataka BazaPodataka { get; }

    public Result<Unit> PokusajKonekciju() => BazaPodataka.IsReady
        ? Result<Unit>.Success(new Unit())
        : Result<Unit>.Failure("baza.neispravan_json", "Podatke nije moguće bezbedno učitati.", new Unit());
}
