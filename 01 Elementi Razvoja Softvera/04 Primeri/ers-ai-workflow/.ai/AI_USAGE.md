# AI_USAGE.md

Ovo je primer kratke evidencije. Ne čuva se ceo razgovor.

## Primer zapisa
- **Zadatak:** proveriti idempotentnost `CreateReservationHandler` use-case-a.
- **Alat:** Kova, model `qwen3:4b`, režim Manual, skill `review-pull-request`.
- **Kontekst:** `CreateReservationHandler.cs`, `IReservationRepository`, `InMemoryInventoryModule`, postojeći NUnit testovi.
- **Predlog AI alata:** pre rezervacije zalihe proveriti da li već postoji rezervacija za isti `RequestId`.
- **Odluka tima:** prihvaćeno; idempotency key pripada aplikacionom use-case-u, ne HTTP endpoint-u.
- **Provera:** test `CreateReservation_WhenRequestIsRepeated_IsIdempotent` i pregled diff-a.
