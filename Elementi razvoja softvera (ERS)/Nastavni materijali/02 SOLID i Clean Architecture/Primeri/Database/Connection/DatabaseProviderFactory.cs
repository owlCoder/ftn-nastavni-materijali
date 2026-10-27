using Database.Connection.Providers;
using Database.Interfaces;

namespace Database.Connection;

/// <summary>
/// Bira provajdera skladišta po imenu. Time se izbor baze drži na jednom mestu
/// i može da se promeni kroz jednu DI tačku, umesto da se "pogađa" tokom pokretanja.
/// </summary>
public static class DatabaseProviderFactory
{
    public static IDatabaseProvider Create(string nazivProvajdera)
    {
        return nazivProvajdera.Trim().ToLowerInvariant() switch
        {
            "json" => new JsonDatabaseProvider(),
            _ => new InMemoryDatabaseProvider(),
        };
    }
}
