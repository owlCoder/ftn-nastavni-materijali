# AI_USAGE.md

Ovo je primer kratke evidencije. Ne čuva se ceo razgovor.

## Primer zapisa
- **Zadatak:** proveriti idempotentnost `CreateReservationHandler` use-case-a.
- **Kontekst:** `CreateReservation.cs`, `IReservationRepository`, `InMemoryInventoryModule`, postojeći NUnit testovi.
- **Predlog AI alata:** pre rezervacije zalihe proveriti da li već postoji rezervacija za isti `RequestId`.
- **Odluka tima:** prihvaćeno; idempotency key pripada aplikacionom use-case-u, ne HTTP controller-u.
- **Provera:** test `CreateReservation_WhenRequestIsRepeated_IsIdempotent` i pregled diff-a.
