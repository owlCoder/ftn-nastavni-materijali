# AI_INSTRUCTIONS.md

Stabilna projektna pravila za rad AI alata nad ovim solution-om. Kova ih ne učitava automatski: svaki skill iz `.kova/skills/` čita ovu datoteku kao prvi korak, a MCP server je izlaže kao resource `project://instructions`.

## Arhitektura
- `Domain` ne zavisi ni od jednog drugog projekta. Modeli nose stanje, a pravila sprovode domenski servisi.
- `Application` zavisi samo od `Domain`. Sadrži use-case-ove, validatore komandi i portove prema spoljnim sistemima.
- `Infrastructure` implementira portove iz `Application` sloja i ne donosi poslovne odluke.
- `Api` i `ConsoleUi` su presentation adapteri i composition root-ovi: mapiraju ulaz i izlaz, a use-case pozivaju preko njegovog interfejsa.
- MCP i guardrails su razvojni alati. Ne smeju postati zavisnost poslovnog jezgra.

## SOLID
- Jedna klasa ima jednu jasnu odgovornost.
- Nova politika/guardrail dodaje se implementacijom interfejsa, ne grananjem kroz postojeće klase.
- Implementacije portova moraju poštovati ugovor interfejsa.
- Interfejsi ostaju mali i vezani za konkretan use-case.
- Visoki slojevi zavise od apstrakcija, a composition root bira konkretne implementacije.

## Ishodi
- Očekivani poslovni neuspeh vraća se kao `Result` sa stabilnim kodom, ne kao izuzetak.
- Novi kod neuspeha dodaje se u odgovarajuću `*ErrorCodes` klasu i pokriva testom.

## Pre izmene
1. Pročitaj zahtev, relevantan kod i testove.
2. Navedi pogođene slojeve i granice modula.
3. Predloži mali plan i rizike.
4. Ne menjaj kod dok plan nije jasan.
5. Navedi testove kojima će rezultat biti proveravan.

## Posle izmene
1. Pokreni ciljane testove.
2. Pokreni kompletan test projekat kada je praktično.
3. Pregledaj `git diff` i ukloni nepovezane izmene.
4. Ne tvrdi da je nešto provereno ako stvarna komanda nije izvršena.
5. Ne čitaj `.env`, tajne ili pristupne tokene.
