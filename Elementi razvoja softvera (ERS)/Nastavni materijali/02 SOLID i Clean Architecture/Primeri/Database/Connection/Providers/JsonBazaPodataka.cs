using Database.Interfaces;
using Database.Models;
using Newtonsoft.Json;

namespace Database.Connection.Providers;

public sealed class JsonBazaPodataka : IBazaPodataka
{
    private readonly string _putanja;

    public TabeleBazaPodataka Tabele { get; private set; } = new();
    public bool IsReady { get; private set; } = true;

    public JsonBazaPodataka(string putanja = "podaci.json")
    {
        _putanja = putanja;
        if (!File.Exists(_putanja))
            return;

        try
        {
            string json = File.ReadAllText(_putanja);
            TabeleBazaPodataka? podaci = JsonConvert.DeserializeObject<TabeleBazaPodataka>(json);
            if (podaci is null || podaci.Korisnici is null || podaci.Objave is null || podaci.Evidencije is null)
            {
                IsReady = false;
                return;
            }

            Tabele = podaci;
        }
        catch (IOException)
        {
            IsReady = false;
        }
        catch (UnauthorizedAccessException)
        {
            IsReady = false;
        }
        catch (JsonException)
        {
            IsReady = false;
        }
    }

    public bool SacuvajPromene()
    {
        if (!IsReady)
            return false;

        string privremenaPutanja = _putanja + ".tmp";
        try
        {
            string json = JsonConvert.SerializeObject(Tabele, Formatting.Indented);
            File.WriteAllText(privremenaPutanja, json);
            File.Move(privremenaPutanja, _putanja, overwrite: true);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
