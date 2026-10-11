# OIB — programski primeri

Bezbednosna pravila, testovi i dokazi primene kroz osam samostalnih C#/.NET primera.

| Vežba | Tema |
|---|---|
| 1 | [Identitet, autentikacija i RBAC](<01 Identitet autentikacija i RBAC/README.md>) |
| 2 | [Autorizacija nad resursom i klasifikacija](<02 Resurs klasifikacija/README.md>) |
| 3 | [Politike, konfiguracija i vidljivost](<03 Politike konfiguracija/README.md>) |
| 4 | [Imovina, granice poverenja i threat modeling](<04 Threat modeling/README.md>) |
| 5 | [MFA i sesije](<05 MFA sesije tajne/README.md>) |
| 6 | [Detekcija i incident](<06 Detekcija incident/README.md>) |
| 7 | [ABAC, rizik i pregled pristupa](<07 ABAC rizik pregled/README.md>) |
| 8 | [Korelacija i efektivnost kontrole](<08 Korelacija efektivnost/README.md>) |

## Pokretanje

Potreban je .NET 8 SDK. Svaki direktorijum sadrži zaseban solution.

```bash
cd "01 Identitet autentikacija i RBAC"
dotnet run --project src/Oib.Vezba01.ConsoleUi
dotnet test Oib.Vezba01.sln
```

Provera svih primera iz ovog direktorijuma: `./verify.sh` na macOS/Linux-u ili sledeća PowerShell komanda na Windows-u:

```powershell
Get-ChildItem -Recurse -Filter *.sln | ForEach-Object { dotnet test $_.FullName --nologo }
```

## Organizacija

`Domain` sadrži modele i pravila; `Application` use-case-ove i portove; `Infrastructure` implementacije portova; `ConsoleUi` sklapa zavisnosti i prikazuje ishod. `Application` zavisi samo od `Domain`. NUnit testovi koriste zamenljive satove i adaptere za ponovljive provere.

README svake vežbe navodi scenario, očekivani dokaz i zadatak za proširenje.

[Prezentacije](<../02 Prezentacije/README.md>) · [Materijali predmeta](<../README.md>)
