# ODP .NET primeri

Osam malih, samostalnih C#/.NET primera prati teme računarskih vežbi iz Osnova distribuiranog programiranja. Svaka vežba je zaseban solution koji može da se otvori, pokrene i testira nezavisno od ostalih. Udaljeni uređaji, mreža i kvarovi su simulirani, pa se svaki scenario može ponoviti bez čekanja i bez prave opreme.

| Vežba | Tema | Odluka koju primer donosi |
|---|---|---|
| 1 | Način razmišljanja u distribuiranom sistemu | šta sistem sme da tvrdi o čvoru koji se ne javlja |
| 2 | Ugovori, simulatori i ponovljivost | da li poruka poštuje ugovor i može li se scenario ponoviti |
| 3 | Identitet, audit, observability i konfiguracija | kako se jedna operacija prati kroz dve komponente |
| 4 | Verzionisanje i failure-first testiranje | šta se radi sa duplikatom, zakasnelom porukom i novom verzijom |
| 5 | Tok podataka i read modeli | da li zakasnelo merenje menja prikaz i kada je prikaz zastareo |
| 6 | Komande, poslovi i idempotentnost | kako komanda napreduje kada potvrda kasni ili izostane |
| 7 | Rad bez veze i pouzdana isporuka | kako namera preživljava prekid veze i izgubljenu potvrdu |
| 8 | Koordinacija, protok i eventualna konzistentnost | ko je vlasnik resursa i šta se dešava kada je red pun |

## Zajednička struktura

Sve vežbe imaju isti raspored projekata i isti smer zavisnosti:

```text
NN Tema/
  Odp.VezbaNN.sln
  Directory.Build.props
  src/
    Odp.VezbaNN.Domain/          modeli i pravila; ne zavisi ni od čega
    Odp.VezbaNN.Application/     use-case i portovi; zavisi samo od Domain
    Odp.VezbaNN.Infrastructure/  adapteri i simulatori koji implementiraju portove
    Odp.VezbaNN.ConsoleUi/       composition root i demonstracija
  tests/
    Odp.VezbaNN.Tests/           NUnit testovi pravila i use-case-a
```

- Model nosi stanje, a distribuirano pravilo je u domenskom servisu ili politici.
- Odluka je eksplicitan rezultat sa stabilnim kodom, ne `bool` i ne izuzetak.
- Vreme, veza, skladišta i udaljene komponente su portovi, pa su testovi deterministički.
- Kvar se zadaje u testu (pomeranjem sata, prekidom veze, gubitkom potvrde), ne čeka se.
- Console UI samo sklapa zavisnosti i prikazuje ishod; ne sadrži pravila.

Svaki `README.md` u vežbi povezuje primer sa lancem **entitet i vlasništvo → ugovor ili identitet operacije → odluka → failure scenario → test → dokaz** i završava se zadatkom za proširenje.

## Pokretanje

```bash
cd "01 Vlasnistvo neizvesnost"
dotnet run --project src/Odp.Vezba01.ConsoleUi
dotnet test Odp.Vezba01.sln
```

Provera svih primera odjednom, na macOS/Linux sistemu:

```bash
./verify.sh
```

Na Windows sistemu, iz PowerShell-a:

```powershell
Get-ChildItem -Recurse -Filter *.sln | ForEach-Object { dotnet test $_.FullName --nologo }
```
