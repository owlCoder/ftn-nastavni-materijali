using Presentation.Interfaces;
using Database.Connection;
using Database.Repositories;
using Database.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Infrastructure.Security;
using Infrastructure.Services;
using Application.Services;
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

    private readonly IZapisiServis _blogServis;
    private readonly IZapisiServis _evidencijaServis;

    private AppContainer(IZapisiServis blogServis, IZapisiServis evidencijaServis)
    {
        _blogServis = blogServis;
        _evidencijaServis = evidencijaServis;
    }

    public IZapisiServis ZapisiServisZa(TipKorisnika tip) => tip == TipKorisnika.Bloger ? _blogServis : _evidencijaServis;

    public static AppContainer Create(string nazivProvajdera)
    {
        IConsoleIO io = new SystemConsoleIO();
        IDatabaseProvider databaseProvider = DatabaseProviderFactory.Create(nazivProvajdera);
        IBazaPodataka baza = databaseProvider.BazaPodataka;

        IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(baza);
        IZapisiRepozitorijum objaveRepozitorijum = new ObjaveRepozitorijum(baza);
        IZapisiRepozitorijum evidencijeRepozitorijum = new EvidencijeRepozitorijum(baza);

        IPasswordHasher passwordHasher = new Pbkdf2PasswordHasher();
        IAutentifikacijaServis authServis = new AutentifikacioniServis(korisniciRepozitorijum, passwordHasher);
        ILoggerServis logger = new FileLoggerServis();
        KorisniciSeeder seeder = new(databaseProvider, authServis);

        IZapisiServis blogServis = new BlogServis(objaveRepozitorijum, logger);
        IZapisiServis evidencijaServis = new EvidencijaServis(evidencijeRepozitorijum, logger);

        return new AppContainer(blogServis, evidencijaServis)
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
