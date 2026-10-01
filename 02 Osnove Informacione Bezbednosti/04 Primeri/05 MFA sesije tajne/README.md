# Vežba 5 — MFA, sesije, servisi i tajne

Primer pokazuje step-up odluku: rizična operacija zahteva svežu MFA potvrdu, a stara, nepostojeća ili opozvana sesija nije dovoljna.

## Pokretanje

```bash
dotnet run --project src/Oib.Vezba05.ConsoleUi
dotnet test Oib.Vezba05.sln
```

## Struktura

```text
src/
  Oib.Vezba05.Domain/          AuthSession, RiskyOperation, StepUpDecision, StepUpAuthenticationPolicy
  Oib.Vezba05.Application/     AuthorizeRiskyOperationHandler + portovi (skladište sesija, sat)
  Oib.Vezba05.Infrastructure/  in-memory skladište sesija, sistemski sat
  Oib.Vezba05.ConsoleUi/       composition root i demonstracija
tests/
  Oib.Vezba05.Tests/           testovi politike i use-case-a
```

## Bezbednosni lanac

| Korak | U primeru |
|---|---|
| Imovina | poverljivi podaci koji se izvoze |
| Pretnja | preuzeta ili davno potvrđena sesija pokreće rizičnu operaciju |
| Zahtev | MFA potvrda ne sme biti starija od granice koju operacija propisuje |
| Kontrola | `StepUpAuthenticationPolicy` |
| Test | `StepUpAuthenticationPolicyTests`, `AuthorizeRiskyOperationHandlerTests` |
| Dokaz | `StepUpDecision` sa stabilnim kodom i oznakom da li se traži MFA |

## SOLID i Clean Architecture

- Vreme je zavisnost: politika prima trenutak kao argument, a handler ga dobija kroz `IClock`, pa su testovi deterministički (DIP).
- Politika ne veruje vremenskoj oznaci iz budućnosti — takva MFA potvrda se tretira kao nevažeća.
- Opozvana sesija se odbija bez ponude MFA koraka: `RequiresMfa` je `false`, jer nova potvrda ne popravlja opoziv.
- Handler zavisi od `ISessionStore`; nepoznata sesija je eksplicitan ishod `SessionNotFound`.

## Zadatak

Dodati maksimalnu starost same sesije: sesija izdata pre više od osam sati odbija se bez obzira na MFA. Uvesti nov kod odluke i test za graničnu vrednost.
