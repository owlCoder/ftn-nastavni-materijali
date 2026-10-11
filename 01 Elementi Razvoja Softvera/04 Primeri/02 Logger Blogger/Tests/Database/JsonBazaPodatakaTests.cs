using Database.Connection.Providers;
using Domain.Models;
using NUnit.Framework;

namespace Tests.Database;

[TestFixture]
public sealed class JsonBazaPodatakaTests
{
    [Test]
    public void NeispravanJson_NePrepisujePostojeciFajl()
    {
        string path = Path.Combine(Path.GetTempPath(), $"logger-blogger-{Guid.NewGuid():N}.json");
        const string corrupted = "{ neispravan json";
        try
        {
            File.WriteAllText(path, corrupted);
            JsonBazaPodataka baza = new(path);
            JsonDatabaseProvider provider = new(path);

            Assert.That(baza.IsReady, Is.False);
            Assert.That(baza.SacuvajPromene(), Is.False);
            Assert.That(provider.PokusajKonekciju().IsFailure, Is.True);
            Assert.That(File.ReadAllText(path), Is.EqualTo(corrupted));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Test]
    public void ValidanJson_CuvaKorisnikeIPonovoUcitava()
    {
        string path = Path.Combine(Path.GetTempPath(), $"logger-blogger-{Guid.NewGuid():N}.json");
        try
        {
            JsonBazaPodataka baza = new(path);
            baza.Tabele.Korisnici.Add(new Korisnik("ana", "hash", "Ana"));

            Assert.That(baza.SacuvajPromene(), Is.True);
            JsonBazaPodataka procitana = new(path);
            Assert.That(procitana.IsReady, Is.True);
            Assert.That(procitana.Tabele.Korisnici.Count, Is.EqualTo(1));
            Assert.That(procitana.Tabele.Korisnici[0].KorisnickoIme, Is.EqualTo("ana"));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
            if (File.Exists(path + ".tmp"))
                File.Delete(path + ".tmp");
        }
    }
}
