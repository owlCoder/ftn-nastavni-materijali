using Database.Connection;
using Database.Repositories;
using Domain.Interfaces.Database;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using Domain.Interfaces.Services;
using Infrastructure.Security;
using Infrastructure.Services;
using Presentation.Console;
using Presentation.Seeding;

namespace Presentation.Composition;

/// <summary>
/// Composition root: jedino mesto u aplikaciji koje zna za konkretne tipove i sastavlja
/// graf zavisnosti. Ostatak koda zavisi isključivo od apstrakcija.
/// </summary>
public sealed class AppContainer
{
    public required IConsoleIO Io { get; init; }
    public required IDatabaseProvider DatabaseProvider { get; init; }
    public required IKorisniciRepozitorijum KorisniciRepozitorijum { get; init; }
    public required IAutentifikacijaServis AuthServis { get; init; }
    public required ILoggerServis Logger { get; init; }
    public required KorisniciSeeder Seeder { get; init; }

    private readonly IZapisiRepozitorijum _objaveRepozitorijum;
    private readonly IZapisiRepozitorijum _evidencijeRepozitorijum;

    private AppContainer(IZapisiRepozitorijum objaveRepozitorijum, IZapisiRepozitorijum evidencijeRepozitorijum)
    {
        _objaveRepozitorijum = objaveRepozitorijum;
        _evidencijeRepozitorijum = evidencijeRepozitorijum;
    }

    /// <summary>Vraća servis za zapise koji odgovara izabranom tipu korisnika.</summary>
    public IZapisiServis ZapisiServisZa(TipKorisnika tip) => tip switch
    {
        TipKorisnika.Bloger => new BlogServis(_objaveRepozitorijum, Logger),
        _ => new EvidencijaServis(_evidencijeRepozitorijum, Logger)
    };

    public static AppContainer Create(string nazivProvajdera)
    {
        IConsoleIO io = new SystemConsoleIO();
        IDatabaseProvider databaseProvider = DatabaseProviderFactory.Create(nazivProvajdera);
        IBazaPodataka baza = databaseProvider.BazaPodataka;

        IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(baza);
        IZapisiRepozitorijum objaveRepozitorijum = new ObjaveRepozitorijum(baza);
        IZapisiRepozitorijum evidencijeRepozitorijum = new EvidencijeRepozitorijum(baza);

        IPasswordHasher passwordHasher = new Sha256PasswordHasher();
        IAutentifikacijaServis authServis = new AutentifikacioniServis(korisniciRepozitorijum, passwordHasher);
        ILoggerServis logger = new FileLoggerServis();
        KorisniciSeeder seeder = new(databaseProvider, authServis);

        return new AppContainer(objaveRepozitorijum, evidencijeRepozitorijum)
        {
            Io = io,
            DatabaseProvider = databaseProvider,
            KorisniciRepozitorijum = korisniciRepozitorijum,
            AuthServis = authServis,
            Logger = logger,
            Seeder = seeder
        };
    }
}
