# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

## Vežba 8 — Korelacija događaja, efektivnost kontrola i učenje

Završna slika bezbednosti ne sastoji se od pojedinačnih kontrola, već od njihove veze. Događaji dobijaju smisao kada se korelišu, kontrola dobija vrednost kada postoji svež dokaz da radi, a incident postaje prilika da se sistem poboljša bez prikrivanja grešaka.

Merenje efektivnosti sprečava lažni osećaj sigurnosti. Kontrola koja postoji, ali je pogrešno podešena, nikada testirana ili bez vlasnika, ne zaslužuje isto poverenje kao kontrola sa proverljivim dokazom.

### Bezbednost kao proces koji se meri i unapređuje

- Događaji: pojedinačni tragovi
- Korelacija: zajednički kontekst
- Kontrola: smanjuje određeni rizik
- Dokaz i učenje: meri i poboljšava

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 8.1. Kontrola ima smisao tek uz dokaz

Merenje efektivnosti sprečava lažni osećaj sigurnosti. Kontrola koja postoji, ali je pogrešno podešena, nikada testirana ili bez vlasnika, ne zaslužuje isto poverenje kao kontrola sa proverljivim dokazom.

- Korelacija povezuje događaje kroz vreme, aktere i resurse, a ne samo po jednoj poruci loga.
- Odgovor sa većim posledicama zahteva ljudsku odluku i jasan trag odobrenja.
- Post-incident pregled ispituje sistem i proces, ne traži najbližeg krivca.

> **Pitanje za diskusiju**
>
> Izaberite jednu kontrolu i pokažite koji rizik ublažava, koji dokaz čuva i kako biste primetili da je prestala da radi.

### 8.2. Kako se ova tema vidi u razvoju

Dobra bezbednosna analiza pokazuje uzročnu vezu: koju imovinu štitimo, koji scenario razmatramo, koja kontrola deluje i kako znamo da njeno ponašanje još uvek odgovara nameri. Kratak, jasan dokaz vredi više od duge liste funkcionalnosti.

#### Česte greške koje treba prepoznati

- Zaključivanje iz jednog događaja bez šireg konteksta.
- Automatska akcija sa velikom posledicom bez zaštitne provere.
- Ocena kontrole samo po tome da li je implementirana.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 8.3. Primer: kontrola postoji, ali nema svež dokaz

Sistem ima pravilo koje ograničava pristup osetljivom resursu. Pravilo je bilo uspešno testirano pre nekoliko meseci, ali se u međuvremenu promenio model uloga i dodat je novi ulaz u sistem. Bez novog dokaza tim zna da kontrola postoji, ali ne zna da li još pokriva stvarni rizik. Efektivnost zato uključuje svežinu, opseg i rezultat provere.

- Korelacija povezuje niz događaja u jedinstven prikaz ponašanja sistema.
- Mera efektivnosti prati da li kontrola ima vlasnika, dokaz i pokrivenost relevantnog scenarija.
- Automatizovan odgovor može ubrzati reakciju, ali radnja sa velikom posledicom zahteva ljudsku potvrdu i revizijski trag.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 8.4. Dizajnerska odluka: korelacija pre zaključka

Pojedinačan događaj često ima više mogućih objašnjenja. Korelacija povezuje vreme, aktera, resurs i tok rada, pa tim može da razlikuje slučajnu grešku od obrasca koji zaslužuje reakciju. Cilj nije prikupiti najviše podataka, već povezati one podatke koji menjaju odluku.

- Korelacioni identifikator povezuje nameru, obradu i ishod jedne radnje.
- Pravila korelacije moraju biti objašnjiva, posebno kada utiču na prioritet upozorenja.
- Automatizacija predlaže ili ograničava odgovor kada je posledica greške velika.

### 8.5. Efektivnost je više od postojanja kontrole

Kontrola može biti implementirana, ali pogrešno podešena, zaobiđena novim tokom ili zastarela posle promene sistema. Zato se njena efektivnost procenjuje kroz dokaz: poslednji uspešan test, pokriveni scenario, vlasnika i sposobnost da se problem primeti ako kontrola zakaže.

- Dokaz se osvežava kada se promeni relevantan tok, uloga ili granica poverenja.
- Mera efektivnosti ne skriva nepoznato, već pokazuje gde dokaza nema dovoljno.
- Kvalitetna analiza povezuje rizik, kontrolu, test i preostalo ograničenje u jednu jasnu priču.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 8.6. Šema kontinuiranog unapređivanja bezbednosti

### Šema kontinuiranog unapređivanja bezbednosti

- Događaji: tragovi ponašanja sistema
- Korelacija: obrazac i prioritet
- Kontrola: odgovor na rizik
- Dokaz i učenje: provera i sledeće poboljšanje

Bezbednost napreduje kroz povratnu spregu: dokaz menja razumevanje rizika, a to menja kontrolu.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 8.7. Dokaz, pokrivenost i efektivnost

| Pitanje | Slab odgovor | Pouzdan odgovor |
| --- | --- | --- |
| Da li kontrola postoji? | Klasa ili podešavanje je prisutno. | Kontrola ima vlasnika, aktivnu konfiguraciju i svrhu. |
| Da li radi? | Jednom je ručno proverena. | Postoji ponovljiv test ili aktuelan operativni dokaz. |
| Šta pokriva? | Pretpostavlja se da važi svuda. | Poznati su scenario, granica i izuzeci. |
| Kako znamo da je oslabila? | Problem se vidi tek posle incidenta. | Metrika, upozorenje ili redovan pregled otkrivaju odstupanje. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 8.8. Bezbednosna povratna sprega

Zreo sistem ne završava bezbednost implementacijom kontrole. Događaji i testovi stvaraju dokaz, dokaz menja procenu rizika, a procena rizika vodi ka boljoj politici ili drugačijoj kontroli. Tako se izbegava lažni osećaj da je jednom završeni posao zauvek dovoljan.

1. Događaj ili test otkriva obrazac koji zahteva pažnju.
2. Korelacija povezuje obrazac sa imovinom, akterom i kontrolom.
3. Tim proverava da li postojeća kontrola pokriva stvarni scenario.
4. Nova odluka stvara ažuriranu politiku, test ili operativnu proceduru.

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Korelacija događaja, efektivnost kontrola i učenje". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 8.9. Izvršivi primer

`EventCorrelator` povezuje događaje u slučajeve. `ControlEffectivenessCalculator` poredi udeo blokiranih pokušaja sa zadatim ciljem; bez pokušaja nema osnova za ocenu efektivnosti.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-08-korelacija-efektivnost
dotnet run --project src/Oib.Vezba08.ConsoleUi
dotnet test Oib.Vezba08.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Kontrola postoji, ali ne blokira potreban udeo rizičnih pokušaja. |
| Testovi | `EventCorrelatorTests`, `ControlEffectivenessCalculatorTests`, `AnalyzeSecurityEventsHandlerTests` |
| Dokaz | `SecurityAnalysisReport` prikazuje povezane slučajeve i rezultate merenja. |

> **Zadatak uz primer**
>
> Dodati vreme događaja i meriti koliko je prošlo od prvog do poslednjeg događaja u slučaju. Proširiti `CorrelationCase` i napisati test za slučaj sa jednim događajem.

### P4 — Operativna i napredna bezbednost — završna odbrana

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.
