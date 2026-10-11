# Vežba 7 — Rad bez veze i pouzdana isporuka

Stanica čuva poruke u outbox-u, a centar evidentira primenjene `MessageId` vrednosti. Slanje staje na prvoj nepotvrđenoj poruci; `InboxFilter` sprečava ponovljeni efekat.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba07.ConsoleUi
dotnet test Odp.Vezba07.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Prekid veze ili izgubljena potvrda posle isporuke. |
| Testovi | Po povratku veze poruke stižu izvornim redom; ponovljena poruka primenjuje se jednom. |
| Dokaz | `FlushReport` prikazuje pokušaj slanja, a centar broj ignorisanih duplikata. |

## Zadatak

Ograničiti veličinu outbox-a (`Capacity`). Kada je pun, novo merenje se odbija kodom `outbox_full` umesto da se tiho izgubi staro. Pravilo pokriti testom u kome veza ostaje u prekidu.

[Pregled svih primera](<../README.md>)
