# Elementi razvoja softvera — Kontrolne tačke

## P1 — Problem, backlog i razvojni tok

Datum: 19.10.

U ovoj fazi ne zahteva se završena arhitektura niti veliki obim implementacije. Tim obrazlaže problem, razlaže rad na proverljive stavke i dokumentuje početni tok zajedničkog razvoja u repozitorijumu.

- Zajednički repozitorijum sa README dokumentom i pristupom svih članova tima.
- Tapiz Boards sadrži backlog sa prioritetima; opis svake priče je jedna rečenica po šablonu „Kao [uloga], želim/hoću/mogu [radnju], kako bih [postigao cilj]“, a koraci realizacije su u zasebnoj čeklisti.
- Najmanje jedan zahtev za spajanje (pull request) sadrži pregled izmena i obrazložene komentare učesnika.
- Tim koristi dogovoreni tok Backlog → Ready → In Progress → Code Review → QA/Verify → Done.

## P2 — Arhitektura i funkcionalno jezgro

Datum: 02.11.

Tim prikazuje arhitektonske granice i najmanje jedan zaokružen slučaj upotrebe, od ulaznog zahteva do poslovnog rezultata, uz razdvojene odgovornosti poslovnog i infrastrukturnog koda.

- Dokumentovana odgovornost svakog sloja i dozvoljeni smer zavisnosti.
- Implementiran je najmanje jedan vertikalni prolaz kroz sistem, od zahteva do rezultata.
- Za očekivane neuspehe definisani stabilni kodovi ili tipovi rezultata, bez generičkih izuzetaka.
- Domenski i aplikacioni sloj mogu se testirati bez pokretanja stvarne baze podataka ili korisničkog interfejsa.

## P3 — Testiranje i manual-core-baseline

Datum: 23.11.

Do ove kontrolne tačke tim samostalno projektuje jezgro sistema i osnovni skup testova. U narednoj fazi AI alati mogu imati veću ulogu, ali se svaki njihov predlog proverava postojećim testovima i pregledom izmena.

- Ključni slučajevi upotrebe imaju testove za uspešne i negativne scenarije.
- Izveštaj o pokrivenosti je pregledan i najmanje jedna rizična grana je obrazložena ili dodatno pokrivena.
- Najmanje jedan bug je najpre reprodukovan testom, a zatim ispravljen.
- Stabilna verzija jezgra je označena Git tag-om `manual-core-baseline`.

## P4 — Razvoj uz podršku AI alata i završna odbrana

Datum: 14.12.

Na završnoj odbrani svaki član tima obrazlaže zahteve, arhitekturu, testove i način na koji su AI alati korišćeni i proveravani tokom razvoja.

- `AGENTS.md` i `AI_USAGE.md` sadrže projektna pravila i reprezentativne zapise odluka.
- Najmanje dve ponovljive procedure ili uloge AI agenata imaju definisane ulaze, izlaze i ograničenja.
- Postoje najmanje tri evaluaciona scenarija, uključujući negativan slučaj.
- Na odbrani svaki član tima objašnjava svoj deo bez oslanjanja na automatski generisan odgovor.
