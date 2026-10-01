# Vežba 1 — Identitet, autentikacija i RBAC

Primer razdvaja identitet, ulogu, dozvolu i odluku. Sistem ne daje pristup samo zato što je korisnik prijavljen: dozvola mora da stigne preko uloge, a svaka odluka ostavlja revizijski zapis.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba01.ConsoleUi
dotnet test Oib.Vezba01.sln
```

## Struktura

```text
src/
  Oib.Vezba01.Domain/          Actor, Role, AccessDecision, RbacPolicy
  Oib.Vezba01.Application/     AuthorizeAccessHandler + portovi (katalog uloga, revizijski trag, sat)
  Oib.Vezba01.Infrastructure/  in-memory katalog uloga, revizijski trag, sistemski sat
  Oib.Vezba01.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba01.Tests/           testovi politike i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | izveštaji (`reports:view`, `reports:export`) |
| Pretnja | prijavljen korisnik izvozi podatke za koje nema odgovornost |
| Zahtev | pristup se odobrava samo kada uloga eksplicitno dodeljuje dozvolu |
| Kontrola | `RbacPolicy` — odbijanje je podrazumevani ishod |
| Test | `RbacPolicyTests`, `AuthorizeAccessHandlerTests` |
| Dokaz | `AccessAuditEntry` za svaku dozvoljenu i odbijenu odluku |

## SOLID i Clean Architecture

- `RbacPolicy` je domenski servis: ne zna odakle dolaze uloge niti gde se čuva revizijski trag.
- `AuthorizeAccessHandler` zavisi od portova `IRoleCatalog`, `IAccessAuditLog` i `IClock` (DIP), pa se katalog može zameniti bazom bez promene pravila.
- Upis i čitanje revizijskog traga su odvojeni interfejsi (`IAccessAuditLog`, `IAccessAuditTrail`) — use-case dobija samo upis (ISP).
- `Actor`, `Role` i `AccessDecision` nose stanje; pravilo je na jednom mestu (SRP).

## Zadatak

Dodati atribut `IsActive` akteru i pravilo da deaktiviran nalog nema pristup iako ima ulogu. Pravilo dodati u `RbacPolicy`, uvesti nov kod odluke i pokriti ga testom.
