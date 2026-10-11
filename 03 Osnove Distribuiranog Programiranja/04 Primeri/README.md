# ODP — programski primeri

Distribuirani ugovori i scenariji otkaza kroz osam samostalnih C#/.NET primera. Mreža i uređaji su simulirani, pa testovi ne zahtevaju udaljenu opremu.

| Vežba | Tema |
|---|---|
| 1 | [Način razmišljanja u distribuiranom sistemu](<01 Vlasnistvo neizvesnost/README.md>) |
| 2 | [Ugovori, simulatori i ponovljivost](<02 Ugovori simulator/README.md>) |
| 3 | [Identitet, audit i konfiguracija](<03 Identitet audit konfiguracija/README.md>) |
| 4 | [Verzionisanje i failure-first testiranje](<04 Verzionisanje failure-first/README.md>) |
| 5 | [Tok podataka i read modeli](<05 Tok podataka read model/README.md>) |
| 6 | [Komande i idempotentnost](<06 Komande idempotentnost/README.md>) |
| 7 | [Rad bez veze i pouzdana isporuka](<07 Rad bez veze outbox/README.md>) |
| 8 | [Koordinacija i kontrola protoka](<08 Koordinacija protok/README.md>) |

## Pokretanje

Potreban je .NET 8 SDK. Svaki direktorijum sadrži zaseban solution.

```bash
cd "01 Vlasnistvo neizvesnost"
dotnet run --project src/Odp.Vezba01.ConsoleUi
dotnet test Odp.Vezba01.sln
```

Provera svih primera iz ovog direktorijuma: `./verify.sh` na macOS/Linux-u ili sledeća PowerShell komanda na Windows-u:

```powershell
Get-ChildItem -Recurse -Filter *.sln | ForEach-Object { dotnet test $_.FullName --nologo }
```

## Organizacija

`Domain` sadrži modele i pravila; `Application` use-case-ove i portove; `Infrastructure` implementacije portova; `ConsoleUi` sklapa zavisnosti i prikazuje ishod. `Application` zavisi samo od `Domain`. NUnit testovi koriste zamenljive satove i adaptere za ponovljive provere.

README svake vežbe navodi scenario, očekivani dokaz i zadatak za proširenje.

[Prezentacije](<../02 Prezentacije/README.md>) · [Materijali predmeta](<../README.md>)
