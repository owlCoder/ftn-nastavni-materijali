# Logger Blogger

Konzolni nastavni primer (.NET 8, NUnit) sa slojevima i eksplicitnim rezultatima operacija.

- `Domain`: isključivo poslovni modeli i enum tipovi (bez logike formatiranja konzole).
- `Application`: registracija, prijava, objave/evidencije, ugovori i interfejsi (repozitorijumi, servisi, hasher).
- `Database`: `IBazaPodataka`, JSON/in-memory adaptori, tehnički model tabela i repozitorijumi.
- `Infrastructure`: PBKDF2 hasher sa salt-om i file logger.
- `Presentation`: konzolni meni i `AppContainer` kao composition root.
- `Tests`: NUnit/Moq testovi slučajeva upotrebe, modela i adaptera.

Pokretanje: `dotnet run --project Presentation`. Testovi: `dotnet test`.
U `Presentation/Program.cs` promenite `in-memory` na `json` po potrebi.
Primer se otvara na korisnicima **maja** i **ana**, lozinka **demo12345** (isključivo demo kredencijali).

Neispravan postojeći JSON fajl ne biva prepisan praznim tabelama; provajder vraća neuspeh. Upis prvo ide u privremeni fajl pa se zamenjuje ciljni fajl. Repozitorijumi vraćaju grešku i povlače promenu u memoriji ako čuvanje ne uspe.

Lozinke su u obliku `pbkdf2-sha256$iteracije$salt$hash`; postojeći SHA-256 test-podaci se **ne mogu automatski prijaviti** (potrebno je ponovno kreiranje korisnika / migracija sa resetovanjem lozinke).

`Result<T>.Failure(code, message, fallback)` zahteva konkretan prazan model ili praznu kolekciju; kod koji poziva metodu proverava `IsSuccess` pre `Value`. Ovo je svesna nastavna konvencija, a ne zahtev SOLID-a.
