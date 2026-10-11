using Application.Contracts;
using Database.Interfaces;
using Application.Interfaces.Services;
using Shared;

namespace Presentation.Seeding;

/// <summary>
/// Inicijalno podizanje aplikacije: provera konekcije ka skladištu i kreiranje podrazumevanih
/// korisnika. Izdvojeno iz menija i auth servisa da svaki tip ima jednu odgovornost (SRP).
/// </summary>
public sealed class KorisniciSeeder
{
    private static readonly RegistracijaZahtev[] PodrazumevaniKorisnici =
    [
        new("maja", "demo12345", "Maja Taron"),
        new("ana", "demo12345", "Ana Taron")
    ];

    private readonly IDatabaseProvider _databaseProvider;
    private readonly IAutentifikacijaServis _authServis;

    public KorisniciSeeder(IDatabaseProvider databaseProvider, IAutentifikacijaServis authServis)
    {
        _databaseProvider = databaseProvider;
        _authServis = authServis;
    }

    public Result<Unit> Seed()
    {
        Result<Unit> konekcija = _databaseProvider.PokusajKonekciju();
        if (konekcija.IsFailure)
        {
            return konekcija;
        }

        foreach (RegistracijaZahtev zahtev in PodrazumevaniKorisnici)
        {
            Result<Domain.Models.Korisnik> rezultat = _authServis.Registracija(zahtev);
            if (rezultat.IsFailure && rezultat.Error.Code != "auth.korisnicko_ime_zauzeto")
            {
                return Result<Unit>.Failure(rezultat.Error.Code, rezultat.Error.Message, new Unit());
            }
        }

        return Result<Unit>.Success(new Unit());
    }
}
