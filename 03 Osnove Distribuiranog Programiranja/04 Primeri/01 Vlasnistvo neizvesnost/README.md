# Vežba 1 — Način razmišljanja u distribuiranom sistemu

Primer razdvaja poslovni entitet od procesa koji ga predstavlja. Kada udaljeni čvor prestane da se javlja, sistem zna samo koliko dugo ćuti; ne zna da li je proces pao, mreža prekinuta ili je poruka izgubljena. Zato status govori ono što sistem zna, a čvor ostaje registrovan.

## Pokretanje

```bash
dotnet run --project src/Odp.Vezba01.ConsoleUi
dotnet test Odp.Vezba01.sln
```

## Struktura

```text
src/
  Odp.Vezba01.Domain/          StationNode, NodeLivenessPolicy, HeartbeatPolicy
  Odp.Vezba01.Application/     ReportHeartbeatHandler, MonitorNodesHandler + portovi (registar, sat)
  Odp.Vezba01.Infrastructure/  in-memory registar čvorova, sistemski i ručni sat
  Odp.Vezba01.ConsoleUi/       composition root i demonstracija
tests/
  Odp.Vezba01.Tests/           testovi politika i use-case-ova
```

## Distribuirani lanac

| Korak | U primeru |
|---|---|
| Entitet i vlasništvo | `StationNode` pripada registru; proces na stanici ga samo predstavlja |
| Neizvesnost | heartbeat nije stigao na vreme, a razlog se ne zna |
| Odluka | `NodeLivenessPolicy` — `Online`, `Suspected` ili `Unreachable` prema dužini tišine |
| Failure scenario | zakasneli heartbeat stiže posle novijeg (`heartbeat_out_of_order`) |
| Test | `NodeLivenessPolicyTests`, `HeartbeatPolicyTests`, testovi handler-a |
| Dokaz | status sa stabilnim kodom i izmerenom tišinom; čvor nije obrisan |

## SOLID i Clean Architecture

- `NodeLivenessPolicy` i `HeartbeatPolicy` su domenski servisi: ne znaju odakle stiže vreme niti gde se čvorovi čuvaju.
- Vreme je port (`IClock`), pa se prekid veze testira pomeranjem sata, bez čekanja.
- Use-case-ovi zavise od porta `INodeRegistry` (DIP); registar se može zameniti bazom bez promene pravila.
- `StationNode` nosi stanje; pravila su u politikama (SRP).

## Zadatak

Dodati status `Draining` za čvor koji je najavio planirano gašenje. Takav čvor ne sme postati `Unreachable` dok traje najavljeni period. Pravilo dodati u `NodeLivenessPolicy`, uvesti nov kod i pokriti ga testom.
