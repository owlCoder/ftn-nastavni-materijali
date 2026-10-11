# Osnove informacione bezbednosti — Kontrolne tačke

## P1 — Identitet, uloge i autorizacija

Datum: 19.10.

U ovoj fazi ne zahteva se završen bezbednosni model. Tim identifikuje subjekte sistema, razdvaja autentikaciju od autorizacije i povezuje stavke backloga sa proverljivim bezbednosnim zahtevima.

- Zajednički repozitorijum sa README dokumentom i dodeljenom projektnom celinom.
- Osnovna autentikacija i RBAC su implementirani; pristup se proverava serverski, ne samo u UI-ju.
- Najmanje jedan negativan test potvrđuje da je pristup tuđem resursu odbijen bez otkrivanja zaštićenih podataka.
- Revizijski zapis beleži uspešne i neuspešne pokušaje pristupa, uključujući aktera, radnju i ciljni resurs.

## P2 — Politike, klasifikacija i referentna konfiguracija

Datum: 02.11.

Tim uspostavlja katalog bezbednosnih pravila povezan sa konkretnim resursima, klasifikacijom podataka i očekivanom konfiguracijom sistema.

- Katalog bezbednosnih politika podržava verzionisanje bez naknadne izmene objavljene verzije.
- Klasifikacija podataka je dodeljena najmanje jednom tipu resursa i utiče na dozvoljeno rukovanje.
- Referentna bezbednosna konfiguracija omogućava otkrivanje odstupanja od očekivanog stanja.
- Tim ume da objasni koje pravilo prioriteta važi kada su dve politike u konfliktu.

## P3 — Testiranje i manual-core-baseline

Datum: 23.11.

Ova kontrolna tačka zaokružuje osnovni nivo sistema. Postojeća struktura i skup testova moraju omogućiti nezavisnu proveru svake naredne izmene, uključujući izmene predložene pomoću AI alata.

- Evidencija imovine sadrži podatke o kritičnosti i vlasniku svakog registrovanog resursa.
- Ključni bezbednosni slučajevi upotrebe imaju testove za uspešne i negativne scenarije.
- Izveštaj o pokrivenosti je pregledan; najmanje jedna rizična grana je obrazložena ili pokrivena testom.
- Stabilna verzija jezgra je označena Git tag-om `manual-core-baseline`.

## P4 — Operativna i napredna bezbednost — završna odbrana

Datum: 14.12.

Na završnoj odbrani svaki član tima obrazlaže imovinu, pretnje, primenjene kontrole, način provere i preostali rizik u okviru dodeljene projektne celine.

- Višefaktorska ili dodatna autentikacija i vremenski ograničen privilegovan pristup implementirani su za rizične operacije.
- Pravilo detekcije i tok obrade incidenta obuhvataju događaj, upozorenje, incident i dokumentovano zatvaranje.
- Postoje najmanje tri evaluaciona ili bezbednosna scenarija, uključujući negativan scenario ili scenario zloupotrebe.
- Na odbrani svaki član tima objašnjava model pretnji, kontrolu i test svoje celine bez oslanjanja na automatski generisan odgovor.
