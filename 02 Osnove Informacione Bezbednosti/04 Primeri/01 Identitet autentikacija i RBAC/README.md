# Vežba 1 — Identitet, autentikacija i RBAC

Autorizacija pristupa izveštajima na osnovu uloga. `RbacPolicy` podrazumevano odbija pristup; prijavljen nalog mora imati traženu dozvolu.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Oib.Vezba01.ConsoleUi
dotnet test Oib.Vezba01.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Uloga bez dozvole `reports:export` pokušava izvoz. |
| Testovi | `RbacPolicyTests`, `AuthorizeAccessHandlerTests` |
| Dokaz | `AccessAuditEntry` beleži i odobren i odbijen pristup. |

## Zadatak

Dodati atribut `IsActive` akteru i pravilo da deaktiviran nalog nema pristup iako ima ulogu. Pravilo dodati u `RbacPolicy`, uvesti nov kod odluke i pokriti ga testom.

[Pregled svih primera](<../README.md>)
