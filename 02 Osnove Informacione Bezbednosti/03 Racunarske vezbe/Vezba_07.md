# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

## Vežba 7 — Atributi, rizik i periodični pregled pristupa

RBAC daje stabilnu osnovu, ali složenije odluke često zavise i od konteksta: pripadnosti, vrste podataka, vremena, stanja resursa ili nivoa rizika. Atributska pravila uvode taj kontekst bez pretvaranja svake moguće kombinacije u novu ulogu.

Registar rizika i redovan pregled pristupa sprečavaju da zastarele odluke ostanu trajne pretpostavke. Evidentiraju preostali rizik, odgovorno lice i rok za ponovno razmatranje odluke.

### Kada uloga više nije dovoljna za dobru odluku

- Atributi: kontekst odluke
- Politika: pravilo koje ih tumači
- Rizik: uticaj i verovatnoća
- Pregled: potvrda da odluka još važi

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 7.1. Pravo pristupa nije zauvek

Registar rizika i redovan pregled pristupa sprečavaju da zastarele odluke ostanu trajne pretpostavke. Evidentiraju preostali rizik, odgovorno lice i rok za ponovno razmatranje odluke.

- Atributi se čitaju iz pouzdanog, jasnog konteksta.
- Isto stanje mora dovesti do istog rezultata politike.
- Rizik uključuje imovinu, pretnju, kontrolu i preostalu izloženost.

> **Pitanje za diskusiju**
>
> Koju odluku ne biste smeli da zasnivate samo na ulozi i koji dodatni atribut joj daje smisao?

### 7.2. Kako se ova tema vidi u razvoju

Tim izdvaja odluke koje ne mogu pošteno da stanu u naziv jedne uloge. Zatim ih zapisuje kao pravilo koje se može testirati sa nekoliko suprotnih konteksta. Pregled pristupa proverava da li je nekadašnja poslovna potreba i dalje stvarna.

#### Česte greške koje treba prepoznati

- Uvođenje atributa bez jasnog izvora i značenja.
- Pravilo koje zavisi od skrivenog stanja i daje neobjašnjiv rezultat.
- Konflikt dužnosti otkriven tek nakon što je šteta moguća.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 7.3. Primer: ista uloga, različit trenutak

Dva operatera imaju istu ulogu, ali jedan radi na incidentu koji mu je dodeljen, dok drugi radi na drugom domenu. Ako politika gleda samo ulogu, obojica bi mogla da odobre istu rizičnu izmenu. Atributi kao što su pripadnost domenu, stanje incidenta i trenutno odobrenje daju odluci kontekst koji naziv uloge sam ne može da nosi.

- Atributska odluka ostaje razumljiva samo ako je izvor svakog atributa poznat i pouzdan.
- Pravilo treba da daje isti rezultat za isti snimak konteksta, kako bi moglo da se testira i objasni.
- Pregled pristupa vraća tim na pitanje da li pravo i dalje ima poslovni razlog, čak i kada je nekada bilo pravilno dodeljeno.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 7.4. Dizajnerska odluka: kontekst bez haosa

Atributi ne treba da postanu skriveni skup izuzetaka koji niko ne razume. Svaki atribut mora imati jasno značenje, izvor i trenutak na koji se odnosi. Politika zatim navodi kako se ti podaci kombinuju, pa odluka može biti ponovljena, testirana i objašnjena bez oslanjanja na intuiciju implementatora.

- Atributi se prikupljaju iz autoritativnih izvora, a ne iz proizvoljnog zahteva klijenta.
- Pravila treba da budu mala i sastavljena tako da se konflikt može objasniti.
- Kontekst odluke se beleži dovoljno da kasniji pregled razume rezultat bez čuvanja osetljivih detalja.

### 7.5. Rizik kao razgovor o odluci

Registar rizika omogućava poređenje uticaja, verovatnoće, postojećih kontrola i preostale izloženosti. Za prihvaćeni rizik beleže se odgovorno lice, obrazloženje i datum narednog pregleda.

- Rizik povezuje konkretnu imovinu, pretnju i kontrolu.
- Pregled pristupa i pregled rizika imaju ritam, vlasnika i očekivani ishod.
- Segregacija dužnosti sprečava kombinacije prava koje bi jednoj osobi dale preveliku kontrolu.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 7.6. Šema kontekstualne odluke

### Šema kontekstualne odluke

- Pouzdani atributi: ko, gde, kada i nad čim
- Politika: pravilo pristupa
- Rizik: uticaj i preostala izloženost
- Pregled: potvrda ili povlačenje prava

Kontekst daje preciznost odluci, a redovan pregled proverava da li odluka i dalje ima smisla.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 7.7. RBAC i ABAC u istoj odluci

| Pristup | Snaga | Ograničenje |
| --- | --- | --- |
| RBAC | Jasne, stabilne odgovornosti. | Teško izražava vreme, vlasništvo i stanje. |
| ABAC | Koristi bogat kontekst odluke. | Traži pouzdane atribute i čitljiva pravila. |
| Kombinacija | Uloga daje osnovu, atributi preciziraju. | Potrebno je pažljivo objasniti prioritet pravila. |
| SoD | Sprečava rizične kombinacije prava. | Mora se proveravati kroz efektivna prava. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 7.8. Od rizika do odluke o prihvatanju

Rizik se ne uklanja uvek u potpunosti. Neke mere bi bile preskupe, neke bi zaustavile važan rad, a neke zavise od spoljne strane. Tada je važno da preostali rizik bude vidljiv, obrazložen i vremenski ograničen, a ne zaboravljen.

1. Opisati imovinu, scenario i posledicu bez opštih formulacija.
2. Navesti postojeće kontrole i šta one zaista pokrivaju.
3. Oceniti šta ostaje nezaštićeno nakon tih kontrola.
4. Odrediti vlasnika koji prihvata rizik i datum sledećeg pregleda.

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Atributi, rizik i periodični pregled pristupa". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 7.9. Izvršivi primer

`RiskBasedAccessPolicy` proverava kontekst kroz skup `IAccessRule` pravila. Pristup zahteva odgovarajuću ulogu, uređaj, lokaciju i prihvatljiv rizik.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-07-abac-rizik-pregled
dotnet run --project src/Oib.Vezba07.ConsoleUi
dotnet test Oib.Vezba07.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Ispravna uloga koristi se sa nepoznatog uređaja ili uz visok rizik. |
| Testovi | `AccessRuleTests`, `RiskBasedAccessPolicyTests`, `EvaluateAccessHandlerTests` |
| Dokaz | `AccessDecision` daje razlog odluke; `AccessReviewItem` određuje rok i vlasnika pregleda. |

> **Zadatak uz primer**
>
> Dodati pravilo radnog vremena: ograničen resurs dostupan je samo radnim danima od 8 do 18 časova. Vreme proslediti kroz kontekst, a ne čitati ga iz sistema unutar pravila.
