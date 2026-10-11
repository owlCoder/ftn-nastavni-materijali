using Domain.Contracts;
using Domain.Enums;
using Domain.Models;
using Domain.Interfaces.Services;
using Presentation.Composition;
using Presentation.TekstGenerator;
using Shared;

namespace Presentation.Console;

/// <summary>
/// Glavni tok aplikacije iza <see cref="IConsoleIO"/> apstrakcije: izbor tipa korisnika,
/// prijava, pa kreiranje i pregled zapisa odgovarajuće vrste.
/// </summary>
public sealed class ConsoleMenu
{
    private readonly IConsoleIO _io;
    private readonly AppContainer _container;
    private readonly Random _random = new();

    public ConsoleMenu(IConsoleIO io, AppContainer container)
    {
        _io = io;
        _container = container;
    }

    public void Run()
    {
        _io.WriteLine();
        _io.WriteLine("=== Logger-Bloger — .NET 8 ===");
        _io.WriteLine($"Aktivno skladište: {_container.DatabaseProvider.Naziv}");

        TipKorisnika tip = OdaberiTipKorisnika();
        IZapisiServis zapisiServis = _container.ZapisiServisZa(tip);

        Korisnik? korisnik = Prijava();
        if (korisnik is null)
        {
            return;
        }

        PrikaziMeni(tip, zapisiServis, korisnik);
    }

    private TipKorisnika OdaberiTipKorisnika()
    {
        while (true)
        {
            _io.Write("Tip korisnika (Loger, Bloger): ");
            string unos = _io.ReadLine().Trim().ToUpperInvariant();

            switch (unos)
            {
                case "LOGER":
                    return TipKorisnika.Loger;
                case "BLOGER":
                    return TipKorisnika.Bloger;
                default:
                    _io.WriteLine("Nepoznat tip korisnika.");
                    break;
            }
        }
    }

    private Korisnik? Prijava()
    {
        while (true)
        {
            _io.Write("Korisničko ime (prazno za prekid): ");
            string korisnickoIme = _io.ReadLine().Trim();
            if (string.IsNullOrEmpty(korisnickoIme))
            {
                return null;
            }

            _io.Write("Lozinka: ");
            string lozinka = _io.ReadLine().Trim();

            Result<Korisnik> rezultat = _container.AuthServis.Prijava(new PrijavaZahtev(korisnickoIme, lozinka));
            if (rezultat.IsSuccess && rezultat.Value is not null)
            {
                _io.WriteLine($"Uspešna prijava: {rezultat.Value.ImePrezime}");
                return rezultat.Value;
            }

            _io.WriteLine($"Greška: {rezultat.Error.Message}");
        }
    }

    private void PrikaziMeni(TipKorisnika tip, IZapisiServis zapisiServis, Korisnik korisnik)
    {
        bool kraj = false;
        while (!kraj)
        {
            _io.WriteLine();
            _io.WriteLine("1. Objava");
            _io.WriteLine("2. Pregled objava");
            _io.WriteLine("3. Odjavite se");
            _io.Write("Opcija: ");

            string opcija = _io.ReadLine().Trim();
            switch (opcija)
            {
                case "1":
                    KreirajZapis(tip, zapisiServis, korisnik);
                    break;
                case "2":
                    PregledZapisa(tip, zapisiServis);
                    break;
                case "3":
                    kraj = true;
                    break;
                default:
                    _io.WriteLine("Nepoznata opcija.");
                    break;
            }
        }
    }

    private void KreirajZapis(TipKorisnika tip, IZapisiServis zapisiServis, Korisnik korisnik)
    {
        ZapisNaSajtu zapis = tip == TipKorisnika.Bloger
            ? new Objava(
                DateTime.Now,
                NasumicanTekst.GetNasumicanOpis(),
                korisnik,
                NasumicanTekst.GetNasumicanNaslov(),
                (TipObjave)_random.Next(1, 4))
            : new Evidencija(
                DateTime.Now,
                NasumicanTekst.GetNasumicnaLogPoruka(),
                korisnik,
                (TipEvidencije)_random.Next(1, 4));

        Result<ZapisNaSajtu> rezultat = zapisiServis.Objavi(zapis);
        if (rezultat.IsSuccess)
        {
            _io.WriteLine(tip == TipKorisnika.Bloger ? "Objava je dodata!" : "Zapis je evidentiran!");
        }
        else
        {
            _io.WriteLine(tip == TipKorisnika.Bloger ? "Objava nije postavljena!" : "Zapis nije evidentiran!");
        }
    }

    private void PregledZapisa(TipKorisnika tip, IZapisiServis zapisiServis)
    {
        string naziv = tip == TipKorisnika.Bloger ? "blogova" : "logova";
        _io.WriteLine();
        _io.WriteLine($"==================================== Pregled {naziv} ====================================");

        Result<IReadOnlyList<ZapisNaSajtu>> rezultat = zapisiServis.PregledZapisa();
        if (rezultat.IsSuccess && rezultat.Value is not null)
        {
            foreach (ZapisNaSajtu zapis in rezultat.Value)
            {
                _io.Write(zapis.ToString());
            }
        }

        _io.WriteLine("===============================================================================================");
    }
}
