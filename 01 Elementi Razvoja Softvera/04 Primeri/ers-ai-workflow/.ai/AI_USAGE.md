# AI_USAGE.md

Kratka evidencija odluka donetih uz AI alat. Ne čuva se ceo razgovor: zapis treba da omogući timu da rekonstruiše šta je traženo, šta je prihvaćeno ili odbijeno i čime je rezultat proveren.

## Oblik zapisa

- **Zadatak:** šta je traženo i u kom obimu.
- **Alat:** alat, model, režim rada i skill.
- **Kontekst:** datoteke i testovi koji su dati agentu.
- **Predlog AI alata:** sažetak predloga.
- **Odluka tima:** prihvaćeno ili odbijeno, sa razlogom.
- **Provera:** stvarno izvršena komanda ili test i pregled diff-a.

## Primer zapisa — prihvaćen predlog

- **Zadatak:** proveriti idempotentnost `CreateReservationHandler` use-case-a.
- **Alat:** Kova, model `qwen3:4b`, režim Manual, skill `review-pull-request`.
- **Kontekst:** `CreateReservationHandler.cs`, `IReservationRepository`, `InMemoryInventoryModule`, postojeći NUnit testovi.
- **Predlog AI alata:** pre rezervacije zalihe proveriti da li već postoji rezervacija za isti `RequestId`.
- **Odluka tima:** prihvaćeno; idempotency key pripada aplikacionom use-case-u, ne HTTP endpoint-u.
- **Provera:** test `CreateReservation_WhenRequestIsRepeated_IsIdempotent` i pregled diff-a.

## Primer zapisa — odbijen predlog

- **Zadatak:** odrediti gde se sprovodi pravilo raspoložive količine opreme.
- **Alat:** Kova, model `qwen3:4b`, režim Plan, skill `architecture-review`.
- **Kontekst:** `InMemoryInventoryModule.cs`, `InventoryReservationService.cs`, `ReservationEndpoints.cs`, `.ai/AI_INSTRUCTIONS.md`.
- **Predlog AI alata:** uporediti traženu i raspoloživu količinu direktno u `InMemoryInventoryModule`, jer adapter već drži stanje zalihe.
- **Odluka tima:** odbijeno; poslovna odluka bi prešla u Infrastructure i svaki novi adapter bi morao da je ponovi. Pravilo ostaje u domenskom servisu `InventoryReservationService`, a adapter ga samo poziva.
- **Provera:** testovi `Reserve_WhenStockIsInsufficient_ReturnsStableErrorCode` i `CreateReservation_WhenInventoryIsInsufficient_RejectsWithoutChangingInventory`.
