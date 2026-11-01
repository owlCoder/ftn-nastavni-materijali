# Logger–Blogger — postavka zadatka

Maja vodi blog, a Ana evidentira aktivnosti softvera. Zadatak je razviti jednu konzolnu aplikaciju čije se ponašanje određuje prema tipu prijavljenog korisnika, primenom Dependency Inversion principa.

## Projekti

- Domain: Korisnik, ZapisNaSajtu, Objava, Evidencija, TipObjave (VLOG, FRIENDS, FAMILY), TipEvidencije (INFO, ERROR, WARNING), Contracts, Result i interfejsi. Nema zavisnosti.
- Infrastructure: AutentifikacioniServis, BlogServis, EvidencijaServis, FileLoggerServis, Sha256PasswordHasher. Zavisi od Domain.
- Database: InMemoryBazaPodataka, JsonBazaPodataka, DatabaseProviderFactory i repozitorijumi. Zavisi od Domain.
- Presentation: ConsoleMenu, IConsoleIO, AppContainer, seeder, Program.cs. Izvršni projekat.

## Ugovori i ponašanje

- ILoggerServis: bool Loguj(string poruka). Beležiti uspešne i neuspešne operacije u log.txt.
- IZapisiServis: Result<ZapisNaSajtu> Objavi(ZapisNaSajtu zapis), Result<IReadOnlyList<ZapisNaSajtu>> PregledZapisa().
- BlogServis prihvata objave, EvidencijaServis evidencije. Servisi koriste injektovane ILoggerServis i IZapisiRepozitorijum.
- IAutentifikacijaServis: Prijava i Registracija.
- TabeleBazaPodataka: Korisnici, Objave, Evidencije.
- IBazaPodataka: Tabele i bool SacuvajPromene(). InMemory baza radi bez perzistencije, JSON baza čita i piše podaci.json (Newtonsoft.Json).
- ConsoleMenu koristi IConsoleIO, a AppContainer sastavlja zavisnosti (composition root).

## Provera

Proveriti prijavu i registraciju, kreiranje i pregled oba tipa zapisa, odbijanje pogrešnog tipa, log.txt, izbor provajdera i JSON perzistenciju pri ponovnom pokretanju.

Napomena: SHA-256 je naveden u izvornom nastavnom zadatku, ali za produkcione lozinke treba koristiti namenski password-hashing algoritam sa salt vrednošću.

Izvor: priloženi PDF loiggerbloger.pdf (15 strana). Detaljna uređena PDF postavka dostupna je u kompletnom paketu materijala.
