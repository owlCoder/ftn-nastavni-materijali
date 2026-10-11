# Refaktorisanje prema Logger Blogger projektu

- `Domain`: poslovni modeli `Reservation`, `InventoryItem`, `EquipmentAvailability` i domenski `InventoryReservationService`. Nema zavisnosti od HTTP adaptera.
- `Application/Interfaces`: samo interfejsi; `Application/Reservations/Create` drži command, result, outcome, error codes, validator i handler; `Application/Inventory` drži query i interni zahtev inventara.
- `Infrastructure`: implementacije portova za skladište, inventar, zaključavanje i identifikatore.
- `Api/Dtos`: request/response i status DTO (obične klase). `ConsoleUi` i `Api` su posebni adapteri sa composition root-om.
- `Mcp` i `Guardrails` su zasebni tehnički moduli i nisu deo poslovnog `Domain`.
- `Result<T>` više ne ima `new()` generičko ograničenje; neuspeh dobija eksplicitan podrazumevani objekat umesto `null`.
- `.editorconfig` održava standardno Visual Studio formatiranje. Nijedan `record` niti `with` se ne koristi u produkcionom C# kodu.

Nije dovoljna samo lokacija foldera da bi projekat bio SOLID; smer zavisnosti i razdvajanje odgovornosti se proveravaju arhitekturnim testovima. Za stvarnu proveru pokrenuti `dotnet build` i `dotnet test`.
