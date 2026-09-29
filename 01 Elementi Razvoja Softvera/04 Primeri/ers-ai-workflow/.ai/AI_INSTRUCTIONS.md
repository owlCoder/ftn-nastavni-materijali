# AI_INSTRUCTIONS.md

## Arhitektura
- `Domain` ne zavisi ni od jednog drugog projekta.
- `Application` zavisi samo od `Domain` i definiše portove/interfejse prema spoljnim sistemima.
- `Infrastructure` implementira portove iz `Application` sloja.
- `Api` je composition root: registruje zavisnosti i mapira transportne modele; ne sadrži poslovna pravila.
- MCP i guardrails su razvojni alati. Ne smeju postati zavisnost poslovnog jezgra.

## SOLID
- Jedna klasa ima jednu jasnu odgovornost.
- Nova politika/guardrail dodaje se implementacijom interfejsa, ne grananjem kroz postojeće klase.
- Implementacije portova moraju poštovati ugovor interfejsa.
- Interfejsi ostaju mali i vezani za konkretan use-case.
- Visoki slojevi zavise od apstrakcija, a composition root bira konkretne implementacije.

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
