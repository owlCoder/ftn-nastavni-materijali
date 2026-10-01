# OIB .NET primeri

Osam malih, samostalnih C#/.NET primera prati teme računarskih vežbi iz Osnova informacione bezbednosti. Svaka vežba je zaseban solution koji može da se otvori, pokrene i testira nezavisno od ostalih.

| Vežba | Tema | Odluka koju primer donosi |
|---|---|---|
| 1 | Identitet, autentikacija i RBAC | da li uloga aktera dodeljuje traženu dozvolu |
| 2 | Autorizacija nad resursom i klasifikacija | da li subjekat sme da pročita baš taj zapis |
| 3 | Politike, konfiguracija i vidljivost | da li je konfiguracija oslabila u odnosu na baseline |
| 4 | Imovina, granice poverenja i threat modeling | koji scenario pretnje zahteva tretman |
| 5 | MFA, sesije, servisi i tajne | da li je MFA potvrda dovoljno sveža za rizičnu operaciju |
| 6 | Detekcija, incident i ranjivosti | da li niz neuspešnih prijava otvara incident |
| 7 | Atributi, rizik i pregled pristupa | da li kontekst dozvoljava pristup i kada se on ponovo pregleda |
| 8 | Korelacija, efektivnost i učenje | da li kontrola stvarno zaustavlja ono što treba |

## Zajednička struktura

Sve vežbe imaju isti raspored projekata i isti smer zavisnosti:

```text
NN Tema/
  Oib.VezbaNN.sln
  Directory.Build.props
  src/
    Oib.VezbaNN.Domain/          modeli i pravila; ne zavisi ni od čega
    Oib.VezbaNN.Application/     use-case i portovi; zavisi samo od Domain
    Oib.VezbaNN.Infrastructure/  adapteri koji implementiraju portove
    Oib.VezbaNN.ConsoleUi/       composition root i demonstracija
  tests/
    Oib.VezbaNN.Tests/           NUnit testovi pravila i use-case-a
```

- Model nosi stanje, a bezbednosno pravilo je u domenskom servisu ili politici.
- Odluka je eksplicitan rezultat sa stabilnim kodom, ne `bool` i ne izuzetak.
- Vreme, identifikatori i skladišta su portovi, pa su testovi deterministički.
- Console UI samo sklapa zavisnosti i prikazuje ishod; ne sadrži pravila.

Svaki `README.md` u vežbi povezuje primer sa lancem **imovina → pretnja → zahtev → kontrola → test → dokaz** i završava se zadatkom za proširenje.

## Pokretanje

```bash
cd "01 Identitet autentikacija i RBAC"
dotnet run --project src/Oib.Vezba01.ConsoleUi
dotnet test Oib.Vezba01.sln
```

Provera svih primera odjednom, na macOS/Linux sistemu:

```bash
./verify.sh
```

Na Windows sistemu, iz PowerShell-a:

```powershell
Get-ChildItem -Recurse -Filter *.sln | ForEach-Object { dotnet test $_.FullName --nologo }
```
