using Domain.Interfaces.Database;
using Newtonsoft.Json;

namespace Database.Connection.Providers;

/// <summary>Skladište koje učitava i perzistira podatke u JSON datoteku.</summary>
public sealed class JsonBazaPodataka : IBazaPodataka
{
    private const string Putanja = "podaci.json";

    public TabeleBazaPodataka Tabele { get; }

    public JsonBazaPodataka()
    {
        try
        {
            if (File.Exists(Putanja))
            {
                string json = File.ReadAllText(Putanja);
                Tabele = JsonConvert.DeserializeObject<TabeleBazaPodataka>(json) ?? new();
            }
            else
            {
                Tabele = new();
            }
        }
        catch
        {
            Tabele = new();
        }
    }

    public bool SacuvajPromene()
    {
        try
        {
            string json = JsonConvert.SerializeObject(
                Tabele,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
            File.WriteAllText(Putanja, json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
