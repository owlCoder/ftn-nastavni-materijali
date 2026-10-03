# Evaluacioni scenariji

Unit test proverava determinističku klasu. Evaluacioni scenario proverava ponašanje agentskog toka nad reprezentativnim ulazom, pa ostaje koristan i kada se promene prompt, skill ili model.

## Oblik scenarija

Svaka `*.json` datoteka u ovom direktorijumu ima ista polja:

| Polje | Značenje |
|---|---|
| `id` | stabilan identifikator scenarija |
| `kind` | `positive` — agent mora nešto da uradi ili prijavi; `negative` — agent mora da odbije ili da stane |
| `goal` | šta scenario štiti, jednom rečenicom |
| `skill`, `mode` | Kova skill iz `.kova/skills/` i režim rada u kom se scenario izvodi |
| `input` | zadatak ili sadržaj koji se daje agentu |
| `expected.must` | ponašanje koje mora da se vidi u odgovoru |
| `expected.mustNot` | ponašanje koje scenario zabranjuje |

| Scenario | Vrsta | Šta štiti |
|---|---|---|
| `review-architecture.json` | positive | poslovna logika ne ostaje neprimećena u API sloju |
| `prompt-injection.json` | negative | nepouzdan sadržaj ne ukida projektna pravila |
| `missing-context.json` | negative | agent ne izmišlja pravilo koje zahtev ne definiše |

## Izvođenje

1. U Kova izabrati skill i režim navedene u scenariju.
2. Zadati sadržaj polja `input` kao zadatak.
3. Uporediti odgovor sa `expected.must` i `expected.mustNot`; scenario prolazi samo ako su sve stavke ispunjene.
4. Ishod zabeležiti u `.ai/AI_USAGE.md`.

Ocenu donosi član tima, ne model koji se ocenjuje. Test `AiWorkflowArtifactsTests` proverava samo ono što je determinističko: da su scenariji potpuni, da pominju postojeći skill i da skup sadrži negativan slučaj.
