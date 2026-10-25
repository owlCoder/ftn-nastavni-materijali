# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

## Vežba 6 — Detekcija, incident i ranjivosti

Bezbednosni događaj nije automatski incident. Sistem najpre prepoznaje signal, zatim ga stavlja u kontekst i tek onda određuje da li je potrebna reakcija. Takav tok sprečava i ignorisanje problema i nepotrebno alarmiranje zbog svakog odstupanja.

Upravljanje incidentom daje strukturu trenutku kada je najlakše izgubiti kontekst. Ranjivost postaje rešiv problem kada je povezana sa imovinom, prioritetom, vlasnikom i dokazom da je korekcija proverena.

### Od signala ka proverljivoj i smirenoj reakciji

- Signal: uočeno odstupanje
- Početna obrada: provera konteksta i uticaja
- Incident: koordinisana reakcija
- Unapređenje: korekcija i prevencija

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 6.1. Signal traži kontekst

Upravljanje incidentom daje strukturu trenutku kada je najlakše izgubiti kontekst. Ranjivost postaje rešiv problem kada je povezana sa imovinom, prioritetom, vlasnikom i dokazom da je korekcija proverena.

- Prag detekcije treba da ima smisao i razlog, ne samo proizvoljnu brojku.
- Zatvaranje upozorenja čuva obrazloženje, jer je to znanje za sledeći sličan slučaj.
- Ranjivost nije rešena promenom statusa, već proverom da je rizik zaista smanjen.

> **Pitanje za diskusiju**
>
> Koji događaj bi u vašem sistemu bio koristan signal, ali ne i dovoljan dokaz incidenta? Koji kontekst nedostaje?

### 6.2. Kako se ova tema vidi u razvoju

Tim bira mali broj signala koji su razumljivi operateru. Za svaki opisuje normalno ponašanje, razlog aktiviranja, odgovornu osobu i bezbedan način provere. Tako se detekcija dizajnira kao proizvodna mogućnost, a ne kao sporedni log.

#### Česte greške koje treba prepoznati

- Upozorenje bez vlasnika i pravila za početnu obradu.
- Automatsko zatvaranje bez dokaza o rešavanju uzroka.
- Skeniranje zavisnosti bez utvrđenog načina procene i praćenja nalaza.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 6.3. Primer: više neuspelih prijava

Niz neuspelih prijava može ukazivati na zaboravljenu lozinku, problem integracije ili pokušaj zloupotrebe. Sam broj neuspeha nije dovoljan za zaključak. Početna obrada uključuje poreklo pokušaja, osetljivost naloga i nastavak obrasca nakon upozorenja.

- Detekcija opisuje obrazac koji zahteva proveru, bez automatskog proglašavanja svake greške incidentom.
- Početna obrada beleži odluku i razlog zatvaranja upozorenja.
- Zatvaranje ranjivosti zahteva dokaz da je izloženost uklonjena ili formalno prihvaćena kao preostali rizik.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 6.4. Dizajnerska odluka: signal nije presuda

Detekcija pokušava da primeti promenu ponašanja, ne da automatski čita nameru korisnika. Kvalitetan signal zato navodi obrazac, prag i kontekst koji nedostaje za konačnu odluku. Time operater može brzo da razlikuje normalan problem rada sistema od potencijalnog bezbednosnog događaja.

- Prag se bira prema normalnom ponašanju sistema, a ne prema slučajnom broju.
- Upozorenje ima ozbiljnost, vlasnika i rok do kog se mora pogledati.
- Incident se otvara kada kontekst pokaže stvarni uticaj ili verovatnu zloupotrebu.

### 6.5. Učenje nakon događaja

Dobar post-incident pregled nije potraga za osobom koja je pogrešila. Njegov cilj je da pronađe gde su nedostajali signal, zaštitna provera, jasna odgovornost ili priprema za oporavak. Rezultat treba da menja sistem ili proces na način koji se može proveriti.

- Hronologija odvaja činjenice od pretpostavki nastalih pod pritiskom.
- Akcije nakon incidenta imaju vlasnika i proverljiv završetak.
- Novi scenario se dodaje u testove ili operativne vežbe, kako se isto znanje ne bi izgubilo.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 6.6. Šema odgovora na bezbednosni događaj

### Šema odgovora na bezbednosni događaj

- Događaj: signal iz sistema
- Početna obrada: kontekst i prioritet
- Reakcija: incident ili dokumentovano zatvaranje
- Unapređenje: novi dokaz i manji rizik

Nije svaki signal incident, ali svaki značajan ishod ostavlja znanje korisno za sledeću odluku.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 6.7. Signal, upozorenje i incident

| Stanje | Šta predstavlja | Potrebna reakcija |
| --- | --- | --- |
| Signal | Sirov događaj ili odstupanje. | Sačuvati kontekst i proveriti pravilo. |
| Upozorenje | Signal koji je prešao definisan prag. | Početna obrada i procena prioriteta. |
| Incident | Potvrđen ili verovatan bezbednosni uticaj. | Koordinisana reakcija i sledljiv tok. |
| Nalaz ranjivosti | Potencijalna izloženost koju treba proceniti. | Vlasnik, rok i dokaz korekcije. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 6.8. Tok upravljanja ranjivošću

Nalaz postaje koristan tek kada se poveže sa stvarnom imovinom i njenom izloženošću. Isti tehnički problem nema isti prioritet na izolovanom razvojnom računaru i na javno dostupnoj usluzi koja obrađuje osetljive podatke.

1. Potvrditi da se nalaz odnosi na konkretnu komponentu i aktivnu verziju.
2. Proceniti uticaj u kontekstu dostupnosti, podataka i granica poverenja.
3. Odrediti korekciju, kompenzacionu kontrolu ili obrazloženo prihvatanje rizika.
4. Zatvoriti nalaz tek kada dokaz potvrdi da je izloženost smanjena.

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Detekcija, incident i ranjivosti". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 6.9. Izvršivi primer

`RepeatedFailedLoginRule` prepoznaje najmanje tri neuspešne prijave u deset minuta. `IncidentFactory` iz signala formira incident sa određenom ozbiljnošću.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-06-detekcija-incident
dotnet run --project src/Oib.Vezba06.ConsoleUi
dotnet test Oib.Vezba06.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Uzastopni pokušaji prijave sa iste adrese. |
| Testovi | `RepeatedFailedLoginRuleTests`, `IncidentFactoryTests`, `DetectIncidentsHandlerTests` |
| Dokaz | `Incident` sadrži identifikator, ozbiljnost, sažetak i vreme otvaranja. |

> **Zadatak uz primer**
>
> Dodati pravilo koje prepoznaje password spraying: ista adresa neuspešno pokušava prijavu na više različitih naloga. Napisati novu `IDetectionRule` klasu i registrovati je u composition root-u.
