# SOLID u OIB primerima

Svaka vežba je samostalno .NET rešenje sa slojevima Domain, Application, Infrastructure i ConsoleUi, kao u ERS primeru Logger/Blogger. Domain sadrži modele, pravila i ugovore za zamenljive domenske postupke; Application povezuje slučajeve upotrebe i zavisi od ugovora; Infrastructure sadrži konkretne izvore i skladišta; ConsoleUi sastavlja konkretne zavisnosti u `CompositionRoot`.

- **S — jedna odgovornost:** pravila, slučajevi upotrebe, čuvanje podataka i prikaz imaju odvojene uloge.
- **O — otvorenost za proširenje:** npr. u vežbi 03 može se dodati `IConfigurationControl`, u 06 `IDetectionRule`, a u 07 `IAccessRule`, bez izmene postojećeg sklopa pravila. Politike i kalkulatori mogu se zameniti implementacijom odgovarajućeg ugovora.
- **L — zamenljivost:** implementacije ugovora treba da poštuju iste uslove i značenje rezultata; u testovima vežbi 07 i 08 primeri zamenskih implementacija proveravaju njihovo povezivanje sa slučajem upotrebe.
- **I — razdvojeni interfejsi:** zavisnosti nude samo operacije potrebne klijentu, na primer odvojeni ugovori za upis i pregled audit zapisa.
- **D — obrtanje zavisnosti:** aplikacioni slučajevi upotrebe dobijaju politike i spoljne servise preko interfejsa; konkretne implementacije biraju se u `CompositionRoot`.

Za pokretanje testova u folderu `Primeri` pojedinačne vežbe potreban je .NET SDK 8: `dotnet test`. Konzolni primer pokreće se komandom `dotnet run --project src/Oib.VezbaNN.ConsoleUi`, uz zamenu `NN` brojem vežbe. ZIP arhive sadrže iste izvorne primere.

## Očekivane greške i bezbedne odluke

Očekivana neispravna ulazna vrednost ili neuspeh operacije predstavlja se rezultatom sa kodom greške, a pozivalac proverava rezultat pre nastavka. Vežbe 03, 04, 06 i 08 koriste `Result<T>`; autorizacione vežbe 01, 02, 05 i 07 koriste svoju odluku ili rezultat zahteva sa kodom razloga i podrazumevano odbijaju neispravan zahtev. Rezultati nisu zamena za odluku o pristupu: neuspešna obrada ne sme se tumačiti kao dozvola ili kao dokaz da je konfiguracija usklađena.

Konkretni kodovi kao `InvalidMeasurement`, `InvalidConfiguration` i `InvalidDetectionConfiguration` olakšavaju testiranje bez oslanjanja na izuzetke. Evidentiranje i zakazivanje takođe vraćaju rezultat; kada je zapis odluke ili pregled pristupa obavezan, neuspeh znači da pristup neće biti odobren. Primer upisa izveštaja prevodi očekivane greške pisanja u rezultat na granici infrastrukture. Ne beležiti lozinke, tokene ili druge tajne u porukama greške i audit zapisima.

Primeri su nastavni i koriste memorijska skladišta; u stvarnom sistemu potrebno je posebno obezbediti trajni audit, transakcije, kontrolu pristupa i obradu neočekivanih grešaka na granicama sistema.
