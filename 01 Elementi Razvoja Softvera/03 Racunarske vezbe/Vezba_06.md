# Elementi razvoja softvera — Praktikum

FTN · Nastavni materijali

## Vežba 6 — Kontrolisan razvoj uz AI: kontekst, instrukcije, skill-ovi i režimi rada

AI alati uvode se nakon što je u Vežbi 5 uspostavljeno razumljivo i testirano jezgro sistema. Rad se nastavlja u rešenju `examples/ers-ai-workflow/EquipmentReservation.sln`. AI deo primera podešen je za **Kova**, lokalnog AI agenta za VS Code, koji konfiguraciju čita iz direktorijuma `.kova/`, dok projektna pravila stoje u `AGENTS.md`. AI alat mora poštovati postojeće arhitektonske granice, a rezultat se proverava izgradnjom projekta, testovima i pregledom izmena.

### Kontrolisan tok nad EquipmentReservation solution-om

- Zadatak: jasan cilj i kriterijumi
- Kontekst: relevantni projekti i testovi
- Instrukcije: AGENTS.md
- Skill + režim: .kova/skills i Plan/Manual
- Provera: build, test i git diff

### 6.1. Priprema: Kova u VS Code-u

Kova je lokalni AI agent za VS Code, dostupan kao ekstenzija `owlcoder.kova-local`. Podrazumevani provajder je Ollama sa modelom `qwen3:4b`, pa se ceo primer može raditi bez slanja koda van računara. Opis u praktikumu odgovara verziji Kova 0.3.2 ili novijoj.

1. Instalirati VS Code 1.100 ili noviji, ekstenziju Kova, Ollama i model `qwen3:4b`. Audit hook iz Vežbe 8 koristi Node.js.
2. Izgraditi solution u Release konfiguraciji, jer Kova pokreće MCP server i guardrail projekat sa `--no-build`.
3. Otvoriti direktorijum `examples/ers-ai-workflow` kao VS Code workspace. Skill-ovi, MCP, hook-ovi i guardrail koriste upravo taj root.
4. Pokrenuti **Kova: Open Chat**, izabrati model, režim rada i skill. Za Qwen3 uključiti Thinking: bez njega model razmišljanje ispisuje kao običan odgovor i ne vraća pozive alata.

Priprema modela, solution-a i workspace-a

```bash
ollama pull qwen3:4b
cd examples/ers-ai-workflow
dotnet build EquipmentReservation.sln --configuration Release
code .
```

| Režim | Šta agent sme |
| --- | --- |
| Plan | Samo alati za čitanje; nema izmena datoteka ni pokretanja komandi. |
| Manual | Čitanje radi direktno; svaki upis i svaki proces traže odobrenje. |
| Edit | Uobičajene izmene u workspace-u rade direktno; procesi traže odobrenje. |
| Auto | Izmene i komande sa liste `kova.commands.allow` rade direktno; ostalo traži odobrenje ili se blokira. |

Na vežbama se koriste režimi Plan i Manual, jer je u njima svaka izmena vidljiva pre izvršenja. Guardrail iz Vežbe 8 važi u svakom režimu. Budžet konteksta, Thinking i lista dozvoljenih komandi menjaju se kroz **Kova: Open Settings**.

> **Drugi provajderi šalju kontekst van računara**
>
> Postavkom `kova.provider` može se izabrati DeepSeek ili OpenAI-kompatibilan endpoint, a ključ se unosi komandom **Kova: Set Provider API Key** i čuva u VS Code SecretStorage, ne u `settings.json`. Upit i priloženi kontekst tada odlaze na taj endpoint, pa pravilo da se tajne i `.env` ne daju agentu postaje još važnije.

### 6.2. Od nejasnog zahteva do proverljivog zadatka

Umesto upita „sredi rezervacije“, zadatak treba da kaže koje ponašanje menjamo, koje slojeve ne smemo da narušimo i kako dokazujemo rezultat.

Primer zadatka vezan za konkretan EquipmentReservation kod

```markdown
# Zadatak
Analiziraj promenu: jedna rezervacija ne sme tražiti više od 5 komada opreme.
Ne menjaj kod u ovoj fazi.

# Ograničenja
- Poslovno pravilo mora ostati u Domain/Application delu.
- API ne sme sadržati poslovnu odluku.
- IInventoryModule ugovor menjaj samo ako je zaista potrebno.
- Postojeći testovi moraju ostati uspešni.

# Vrati
1. pogođene fajlove i slojeve,
2. pretpostavke i rizike,
3. minimalni plan izmene,
4. test scenarije,
5. komande za proveru solution-a.
```

> **Prvo analiza, zatim izmena**
>
> Veliki diff nastao iz nejasnog upita je teško pregledati. Plan se pregleda pre nego što agent dobije dozvolu za pisanje.

### 6.3. Projektne instrukcije su verzionisana pravila

Projektna pravila stoje u datoteci `AGENTS.md` u korenu repozitorijuma. To je otvorena konvencija za instrukcije AI agentima koju prepoznaje više alata, pa pravila ostaju ista i kada se alat promeni. Pravila u primeru su konkretna za arhitekturu ovog solution-a i mogu se proveriti čitanjem project reference-a i pokretanjem testova.

> **Kova i AGENTS.md**
>
> Kova od verzije 0.3.2 učitava `AGENTS.md` iz korena projekta u svako pokretanje, pre izabranog skill-a, i to prikazuje u listi aktivnosti kao *Project rules · AGENTS.md*. Skill-ovi zato ne ponavljaju projektna pravila i ne čitaju ih sami. Datoteka sme da ima najviše 16.000 bajtova; veću Kova preskače uz poruku, a test `AiWorkflowArtifactsTests` pada čim se ta granica pređe.

examples/ers-ai-workflow/AGENTS.md

```markdown
## Komande
- Izgradnja: dotnet build EquipmentReservation.sln --configuration Release
- Testovi: dotnet test EquipmentReservation.sln --configuration Release --no-build
- Pregled izmena: git diff -- .

## Arhitektura
- Domain ne zavisi ni od jednog drugog projekta. Modeli nose stanje,
  a pravila sprovode domenski servisi.
- Application zavisi samo od Domain. Sadrži use-case-ove, validatore i portove.
- Infrastructure implementira portove i ne donosi poslovne odluke.
- Api i ConsoleUi su presentation adapteri i composition root-ovi.
- MCP i guardrails su razvojni alati i ne smeju postati zavisnost poslovnog jezgra.

## Ishodi
- Očekivani poslovni neuspeh vraća se kao Result sa stabilnim kodom.

## Posle izmene
1. Pokreni ciljane testove.
2. Pokreni kompletan test projekat kada je praktično.
3. Pregledaj git diff i ukloni nepovezane izmene.
4. Ne tvrdi da je nešto provereno ako stvarna komanda nije izvršena.
5. Ne čitaj .env, tajne ili pristupne tokene.
```

`.ai/AI_USAGE.md` čuva sažet trag: zadatak, korišćeni alat i režim, kontekst, predlog modela, odluku tima i nezavisan dokaz provere. Potpuni chat transcript nije zamena za inženjersku evidenciju. Gotov primer sadrži jedan prihvaćen i jedan odbijen predlog, jer zapis ima vrednost tek kada pokazuje i razlog odluke.

examples/ers-ai-workflow/.ai/AI_USAGE.md

```markdown
## Primer zapisa — odbijen predlog

- **Zadatak:** odrediti gde se sprovodi pravilo raspoložive količine opreme.
- **Alat:** Kova, model qwen3:4b, režim Plan, skill architecture-review.
- **Kontekst:** InMemoryInventoryModule.cs, InventoryReservationService.cs,
  ReservationEndpoints.cs, AGENTS.md.
- **Predlog AI alata:** uporediti traženu i raspoloživu količinu direktno
  u InMemoryInventoryModule, jer adapter već drži stanje zalihe.
- **Odluka tima:** odbijeno; poslovna odluka bi prešla u Infrastructure
  i svaki novi adapter bi morao da je ponovi. Pravilo ostaje u domenskom
  servisu InventoryReservationService, a adapter ga samo poziva.
- **Provera:** testovi Reserve_WhenStockIsInsufficient_ReturnsStableErrorCode
  i CreateReservation_WhenInventoryIsInsufficient_RejectsWithoutChangingInventory.
```

### 6.4. Skill za ponovljiv pregled pull request-a

Kova otkriva skill-ove samo u `.kova/skills/<naziv>/SKILL.md`. Procedura `.kova/skills/review-pull-request/SKILL.md` razdvaja review od implementacije. Isti postupak može da se primeni na više izmena u solution-u.

Sažeta verzija procedure iz gotovog primera

```markdown
---
name: review-pull-request
description: Review an ERS change against requirements, architecture and observed test results.
---

# review-pull-request

## Režim rada
Koristi se u Kova režimu Manual.

## Ulazi
- zahtev i kriterijumi prihvatanja
- projektna pravila iz AGENTS.md (Kova ih učitava automatski)
- git diff (MCP alat get_git_diff)
- rezultat testova (MCP alat run_unit_tests)

## Postupak
1. Sažmi očekivano ponašanje.
2. Proveri da li diff izlazi iz obima zahteva.
3. Proveri Dependency Rule i granice modula.
4. Pregledaj negativne i granične scenarije.
5. Uporedi promenjeno ponašanje sa testovima.
6. Prijavi nalaze po ozbiljnosti.

## Ograničenje
Ne menjaj kod tokom review faze.
```

- Zaglavlje sadrži samo `name` i `description`; naziv mora da odgovara nazivu direktorijuma.
- Skill se bira iz liste **Skill** iznad polja za unos; u kontekst modela ulazi samo izabrani skill.
- Posle dodavanja novog skill-a pokrenuti **Developer: Reload Window** i ponovo otvoriti Kova; izbor **None** uklanja skill iz razgovora.
- Skill je proceduralni kontekst: ne može da promeni dozvole, odobri alat niti izmeni zaštićenu konfiguraciju.

> **Podela odgovornosti**
>
> Uloga namenjena pregledu ne treba istovremeno da bude autor izmene koju ocenjuje. Razdvajanje uloga omogućava nezavisniju proveru rezultata.

### 6.5. Specijalizovane uloge i najmanje privilegije

Uloga je u primeru par skill + režim rada. Skill opisuje postupak, a režim određuje šta agent tehnički sme: u režimu Plan Kova izlaže samo alate za čitanje, dok u režimu Manual svaka izmena i svaka komanda traže odobrenje.

| Skill u primeru | Kova režim i dozvole | Ograničenje |
| --- | --- | --- |
| architecture-review | Plan: čitanje solution-a, strukture, instrukcija i diff-a. | Ne piše kod; režim to sprovodi nezavisno od modela. |
| implement-approved-plan | Manual: menja samo fajlove iz usvojenog plana, uz odobrenje svake izmene. | Ne proširuje poslovni zahtev. |
| review-pull-request | Manual: čita diff, a `run_unit_tests` pokreće uz odobrenje. | Ne menja test samo da sakrije grešku. |

Strukturirana predaja zadatka skill-u implement-approved-plan

```json
{
  "task": "Implement approved reservation quantity rule",
  "constraints": [
    "Keep business rule outside Api",
    "Do not add Infrastructure dependency to Domain/Application"
  ],
  "filesToConsider": [
    "src/EquipmentReservation.Application/Reservations/Create/CreateReservationCommandValidator.cs",
    "src/EquipmentReservation.Application/Reservations/Create/CreateReservationErrorCodes.cs",
    "tests/EquipmentReservation.Tests/Application/CreateReservationCommandValidatorTests.cs"
  ],
  "verification": [
    "dotnet build EquipmentReservation.sln",
    "dotnet test EquipmentReservation.sln --no-build"
  ]
}
```

### 6.6. AI rezultat nije dokaz

Ocena modela nije dokaz ispravnosti promene. Završetak zadatka zahteva uspešnu izgradnju rešenja, prolazak NUnit testova i pregled konačnog skupa izmena.

Minimalna nezavisna provera nakon AI izmene

```bash
dotnet build EquipmentReservation.sln --configuration Release
dotnet test EquipmentReservation.sln --configuration Release --no-build
git diff -- .
```

- Ako komanda nije izvršena, rezultat se ne beleži kao uspešna provera.
- Ako test padne, ne menja se očekivanje testa bez razumevanja poslovnog pravila.
- Ako diff dodiruje slojeve koji nisu bili u planu, promena se vraća na analizu.
- Ako agent traži tajne ili .env, zahtev se odbija i prelazi na guardrail temu iz Vežbe 8.

### 6.7. Rad na vežbi — razvojni tok uz podršku AI alata

> **Zadatak**
>
> Na kopiji `EquipmentReservation.sln` zadati malu promenu poslovnog pravila. Prvo koristiti skill `architecture-review` u režimu Plan samo za analizu; zatim skill-u `implement-approved-plan` u režimu Manual proslediti usvojen plan. Na kraju pokrenuti solution build/test, pregledati diff i uneti sažet zapis u `.ai/AI_USAGE.md`.

| Dokaz | Šta student pokazuje |
| --- | --- |
| Plan pre izmene | Da je razumeo pogođene slojeve i granice. |
| Ograničen skup izmena | Da AI agent nije proširio obim zadatka van usvojenog plana. |
| Build + test rezultat | Da provera nije zasnovana na tvrdnji modela. |
| AI_USAGE zapis | Da tim može rekonstruisati odluku, razlog prihvatanja ili odbijanja predloga i dokaz provere. |

> **Ishod vežbe**
>
> Student ume da koristi AI alat uz očuvanje SOLID principa, arhitektonskih granica i sopstvene odgovornosti za konačan rezultat.

### Kontrolna tačka P3 — Testiranje i manual-core-baseline

Do ove kontrolne tačke tim samostalno projektuje jezgro sistema i osnovni skup testova. U narednoj fazi AI alati mogu imati veću ulogu, ali se svaki njihov predlog proverava postojećim testovima i pregledom izmena.

- Ključni slučajevi upotrebe imaju testove za uspešne i negativne scenarije.
- Izveštaj o pokrivenosti je pregledan i najmanje jedna rizična grana je obrazložena ili dodatno pokrivena.
- Najmanje jedan bug je najpre reprodukovan testom, a zatim ispravljen.
- Stabilna verzija jezgra je označena Git tag-om `manual-core-baseline`.

> **Termin i obuhvat**
>
> Vežba 5–6 · nedelja od 23.11. Isti zahtevi prikazani su na kartici „Kontrolne tačke“.
