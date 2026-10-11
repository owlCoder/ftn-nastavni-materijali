# Vežba 1 — Način razmišljanja u distribuiranom sistemu

`StationNode` je poslovni entitet; proces na stanici ga predstavlja. `NodeLivenessPolicy` određuje `Online`, `Suspected` ili `Unreachable` prema trajanju tišine, bez pretpostavke o uzroku prekida.

## Pokretanje

Potreban je .NET 8 SDK. Komande se izvršavaju iz ovog direktorijuma.

```bash
dotnet run --project src/Odp.Vezba01.ConsoleUi
dotnet test Odp.Vezba01.sln
```

## Scenario i provera

| Stavka | Sadržaj |
|---|---|
| Scenario | Zakasneli heartbeat stiže posle novijeg (`heartbeat_out_of_order`). |
| Testovi | `NodeLivenessPolicyTests`, `HeartbeatPolicyTests` i testovi handler-a |
| Dokaz | Status prikazuje stabilan kod i izmerenu tišinu; registrovani čvor ostaje sačuvan. |

## Zadatak

Dodati status `Draining` za čvor koji je najavio planirano gašenje. Takav čvor ne sme postati `Unreachable` dok traje najavljeni period. Pravilo dodati u `NodeLivenessPolicy`, uvesti nov kod i pokriti ga testom.

[Pregled svih primera](<../README.md>)
