# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

## 0.1. Kako koristiti praktikum

Praktikum je samostalan materijal za razumevanje distribuiranih sistema. Svaka vežba objašnjava problem koji raspodela sistema uvodi, razlog zbog kog je izabrani princip važan, tipične greške i način na koji se odluka proverava u razvoju softvera. Cilj je da student nakon vežbe može samostalno da obnovi princip i primeni ga na dodeljenoj projektnoj celini, a ne da zapamti jedan konkretan primer.

> **Od metode do projekta**
>
> Praktikum ne daje gotovo distribuirano rešenje za projekat. Svaki tim dobija dodeljenu projektnu celinu (npr. telemetriju, komande, koordinaciju ili replikaciju) i princip sa vežbe primenjuje na sopstveni domen, uz obrazloženje odluke na projektnoj kontrolnoj tački.

| Faza | Preporučeni način rada |
| --- | --- |
| Pre vežbe | Pročitati temu i označiti pretpostavke koje mreža, vreme ili više instanci mogu da pokvare. |
| Tokom vežbe | Pratiti razlog odluke i njen trade-off, a ne samo konačni oblik implementacije. |
| Posle vežbe | Povezati princip sa konkretnim distribuiranim scenarijem i objasniti dokaz kroz test ili kontrolisani failure scenario. |
| Pred odbranu | Objasniti granicu odgovornosti, neizvestan ishod i razlog zbog kog sistem na njega reaguje baš tako. |

> **Izvršivi primeri**
>
> Svaku vežbu prati mali, samostalan C#/.NET primer iz direktorijuma `examples/odp-dotnet`. Primer nije deo projekta: on na jednoj odluci pokazuje princip, failure scenario i test koji taj scenario zadaje. Odeljak „Izvršivi primer“ na kraju svake vežbe navodi pravilo u kodu, test i ispis demonstracije.

> **Simulatori umesto realne opreme**
>
> Praktikum ne zahteva realnu orbitalnu mehaniku, radio opremu ni pravu satelitsku komunikaciju. Udaljeni uređaji, mreža i failure situacije reprodukuju se kontrolisanim, ponovljivim simulatorima.

> **Jezik i alati**
>
> Primeri su pretežno u C#/.NET okruženju. Druga tehnologija može biti odobrena kada tim obezbedi interoperabilnost i ekvivalentan nivo testiranja i failure pokrivenosti.

## 0.2. Tok gradiva

Osam vežbi prati tok razmišljanja potreban za distribuirani sistem: najpre vlasništvo i granice, zatim ugovori i vidljivost, potom rad sa porukama, komande i prekidi veze, a na kraju koordinacija, opterećenje i konzistentnost. Redosled gradi mentalni model distribuiranog ponašanja.

- Vlasništvo i granice: poslovni entiteti, čvorovi i neizvesnost mreže.
- Ugovori i vidljivost: poruke, simulatori, korelacija i konfiguracija.
- Pouzdani tokovi: podaci, komande, poslovi, retry i idempotentnost.
- Rad pod pritiskom: prekidi veze, koordinacija, backpressure i zastareli prikazi.

## Vežba 1 — Način razmišljanja u distribuiranom sistemu

Distribuirani sistem nije obična aplikacija raspoređena na više procesa. Komponente posmatraju svet iz različitih trenutaka, mreža može kasniti ili prekinuti vezu, a potvrda operacije može stići nakon što je lokalni kontekst već promenjen. Zato dizajn počinje jasnim vlasništvom i granicama odgovornosti.

Kada se zna koja komponenta je nadležna za podatak ili operaciju, lakše je objasniti šta se dešava pri prekidu veze i ko donosi konačnu odluku. To sprečava da više instanci tiho rade istu stvar ili da se nestanak procesa pogrešno protumači kao nestanak poslovnog entiteta.

### Vlasništvo, granice i neizvesnost pre prve mrežne poruke

- Poslovni entitet: stabilan kroz vreme
- Čvor ili proces: privremena izvršna instanca
- Veza: može kasniti ili nestati
- Vlasništvo: jasna nadležnost

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 1.1. Sistem ne vidi sve u isto vreme

- Proces, mrežna veza i poslovni entitet nisu ista stvar.
- Timeout znači da odgovor nije stigao na vreme, ne da se operacija sigurno nije desila.
- Status treba da izrazi ono što sistem zna, a ne ono što pretpostavlja.

> **Pitanje za diskusiju**
>
> Koji podatak ili operacija mora imati jasan autoritet i kako sistem prepoznaje da je taj autoritet privremeno nedostupan?

### 1.2. Kako se ova tema vidi u razvoju

Tim najpre opisuje granice sistema, odgovornost svake komponente i stanje koje je za nju autoritativno. Takav opis kasnije određuje gde se rade provere, gde se čuva istorija i šta se testira kada čvor privremeno izgubi vezu.

#### Česte greške koje treba prepoznati

- Izjednačavanje prekida veze sa trajnim kvarom.
- Dve instance koje bez pravila menjaju isto stanje.
- Poslovno pravilo skriveno u komunikacionom detalju.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 1.3. Primer: prekid veze nije nestanak stanja

Centralna komponenta prestaje da prima heartbeat od udaljenog čvora. Ona zna da potvrda nije stigla, ali ne zna da li je proces prestao, mreža prekinuta ili je poruka izgubljena. Razlika je važna jer pogrešna pretpostavka može pokrenuti dupli rad ili pogrešno zatvoriti poslovni tok.

- Status treba da izrazi poslednje pouzdano saznanje sistema.
- Ponovno povezivanje ne sme automatski značiti da je nastala nova poslovna instanca.
- Promena vlasništva nad radom mora imati pravilo koje sprečava stari čvor da nastavi sa radom.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 1.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 1.5. Šema autoriteta i neizvesnosti

### Šema autoriteta i neizvesnosti

- Poslovno stanje: stabilna činjenica sistema
- Autoritet: komponenta koja odlučuje
- Mreža: potvrda može kasniti
- Vidljiv status: ono što sistem pouzdano zna

Proces i veza mogu nestati, ali poslovno stanje zahteva jasan i stabilan autoritet.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 1.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Poslovni entitet | stabilan kroz vreme | Šta se dešava kada poslovni entitet nije dostupan ili daje zastarelu informaciju? |
| Čvor ili proces | privremena izvršna instanca | Šta se dešava kada čvor ili proces nije dostupan ili daje zastarelu informaciju? |
| Veza | može kasniti ili nestati | Šta se dešava kada veza nije dostupan ili daje zastarelu informaciju? |
| Vlasništvo | jasna nadležnost | Šta se dešava kada vlasništvo nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 1.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Proces, mrežna veza i poslovni entitet nisu ista stvar.
2. Timeout znači da odgovor nije stigao na vreme, ne da se operacija sigurno nije desila.
3. Status treba da izrazi ono što sistem zna, a ne ono što pretpostavlja.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koji podatak ili operacija mora imati jasan autoritet i kako sistem prepoznaje da je taj autoritet privremeno nedostupan?

### 1.8. Izvršivi primer

Primer razdvaja poslovni entitet od procesa koji ga predstavlja. Kada udaljeni čvor prestane da se javlja, sistem zna samo koliko dugo ćuti; ne zna da li je proces pao, mreža prekinuta ili je poruka izgubljena. Zato status govori ono što sistem zna, a čvor ostaje registrovan.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-01-vlasnistvo-neizvesnost
dotnet run --project src/Odp.Vezba01.ConsoleUi
dotnet test Odp.Vezba01.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-01-vlasnistvo-neizvesnost/src/Odp.Vezba01.Domain/Liveness/NodeLivenessPolicy.cs

```csharp
public NodeLivenessAssessment Evaluate(StationNode node, DateTimeOffset now)
{
    ArgumentNullException.ThrowIfNull(node);

    var silence = SilenceOf(node, now);
    if (silence < _thresholds.SuspectAfter)
        return new(NodeStatus.Online, NodeLivenessCodes.HeartbeatFresh, silence);
    if (silence < _thresholds.UnreachableAfter)
        return new(NodeStatus.Suspected, NodeLivenessCodes.HeartbeatLate, silence);
    return new(NodeStatus.Unreachable, NodeLivenessCodes.HeartbeatMissing, silence);
}

private static TimeSpan SilenceOf(StationNode node, DateTimeOffset now) =>
    now > node.LastHeartbeatAt ? now - node.LastHeartbeatAt : TimeSpan.Zero;
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-01-vlasnistvo-neizvesnost/tests/Odp.Vezba01.Tests/Application/MonitorNodesHandlerTests.cs

```csharp
[Test]
public void Assess_WhenNodeStaysSilent_ReportsItUnreachableButKeepsItRegistered()
{
    var clock = new ManualClock(Start);
    var registry = new InMemoryNodeRegistry([new StationNode("node-ns-1", "GS-NOVI-SAD", Start)]);
    var handler = new MonitorNodesHandler(
        registry,
        new NodeLivenessPolicy(new LivenessThresholds(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(90))),
        clock);

    clock.Advance(TimeSpan.FromMinutes(10));
    var view = handler.Assess().Single();

    using (Assert.EnterMultipleScope())
    {
        Assert.That(view.Liveness.Status, Is.EqualTo(NodeStatus.Unreachable));
        Assert.That(view.Liveness.Code, Is.EqualTo(NodeLivenessCodes.HeartbeatMissing));
        Assert.That(registry.Find("node-ns-1"), Is.Not.Null);
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba01.ConsoleUi

```text
POČETAK [09:00:00]
  node-bg-1 (GS-BEOGRAD): Online | heartbeat_fresh | tišina 0 s
  node-ns-1 (GS-NOVI-SAD): Online | heartbeat_fresh | tišina 0 s
  heartbeat node-ns-1 poslat u 09:00:45 -> heartbeat_accepted
POSLE 45 s (javio se samo node-ns-1) [09:00:45]
  node-bg-1 (GS-BEOGRAD): Suspected | heartbeat_late | tišina 45 s
  node-ns-1 (GS-NOVI-SAD): Online | heartbeat_fresh | tišina 0 s
POSLE 105 s [09:01:45]
  node-bg-1 (GS-BEOGRAD): Unreachable | heartbeat_missing | tišina 105 s
  node-ns-1 (GS-NOVI-SAD): Suspected | heartbeat_late | tišina 60 s
  heartbeat node-ns-1 poslat u 09:00:20 -> heartbeat_out_of_order
STIGAO ZAKASNELI HEARTBEAT ZA node-ns-1 [09:01:45]
  node-bg-1 (GS-BEOGRAD): Unreachable | heartbeat_missing | tišina 105 s
  node-ns-1 (GS-NOVI-SAD): Suspected | heartbeat_late | tišina 60 s
  heartbeat node-bg-1 poslat u 09:01:45 -> heartbeat_accepted
node-bg-1 SE PONOVO JAVIO [09:01:45]
  node-bg-1 (GS-BEOGRAD): Online | heartbeat_fresh | tišina 0 s
  node-ns-1 (GS-NOVI-SAD): Suspected | heartbeat_late | tišina 60 s
```

#### Šta treba uočiti

- Status zavisi samo od dužine tišine; razlog tišine sistem ne zna i ne pretpostavlja.
- Čvor koji je `Unreachable` ostaje u registru: nestanak procesa nije nestanak entiteta.
- Zakasneli heartbeat dobija kod `heartbeat_out_of_order` i ne vraća poznato stanje unazad.

> **Zadatak uz primer**
>
> Dodati status `Draining` za čvor koji je najavio planirano gašenje. Takav čvor ne sme postati `Unreachable` dok traje najavljeni period. Pravilo dodati u `NodeLivenessPolicy`, uvesti nov kod i pokriti ga testom.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Način razmišljanja u distribuiranom sistemu". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

## Vežba 2 — Ugovori, simulatori i ponovljivost

Komponente mogu da sarađuju samo ako dele ugovor o obliku poruke i očekivanom ponašanju. Ugovor nije sporedna dokumentacija: on je granica koja omogućava nezavisnu promenu i daje jasan odgovor na pitanje da li poruka ima značenje za primaoca.

Simulator daje timu ponovljiv način da proveri tokove bez oslanjanja na realnu opremu, nestabilnu mrežu ili slučajne ulaze. Ponovljivost je uslov da se greška razume i da popravka zaista bude dokazana.

### Zajednički jezik komponenti i bezbedan način učenja iz greške

- Ugovor: oblik i značenje poruke
- Izvor: šalje prema ugovoru
- Simulator: ponavlja poznat scenario
- Primalac: validira i tumači

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 2.1. Poruka mora da ima zajedničko značenje

- Objavljena verzija ugovora čuva očekivanja postojećih korisnika.
- Validacija odbija poruku koju sistem ne razume, umesto da joj nagađa značenje.
- Simulator odvaja stvarni problem sistema od slučajnosti spoljnog sveta.

> **Pitanje za diskusiju**
>
> Koju poruku biste najpre pretvorili u stabilan ugovor i koji scenario simulatora bi najbolje otkrio nesporazum između pošiljaoca i primaoca?

### 2.2. Kako se ova tema vidi u razvoju

Tim zapisuje mali skup poruka i scenarija koji predstavljaju ključne tokove. Zatim pokreće isti scenario više puta, sa poznatim ulazima, i proverava da svaki put dobija isti relevantan rezultat.

#### Česte greške koje treba prepoznati

- Promena poruke bez verzije i bez razmišljanja o primaocima.
- Simulator koji sadrži poslovnu logiku centralnog sistema.
- Test koji zavisi od slučajnog vremena ili spoljne usluge.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 2.3. Primer: primalac ne razume novu poruku

Pošiljalac proširuje poruku novim poljem i smatra da promena nema posledice. Stariji primalac, međutim, drugačije tumači vrednost ili odbacuje poruku. Ugovor čuva obe strane tako što oblik, verziju i pravila kompatibilnosti čini eksplicitnim pre nego što poruka stigne do produkcionog toka.

- Verzija govori primaocu kako da tumači poruku.
- Simulator može ponoviti isti tok sa starim i novim primaocem.
- Nejasna ili nepoznata poruka dobija kontrolisan ishod, a ne proizvoljno tumačenje.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 2.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 2.5. Šema ugovora između komponenti

### Šema ugovora između komponenti

- Pošiljalac: stvara poruku
- Ugovor: oblik, značenje i verzija
- Simulator: ponovljiv scenario
- Primalac: validira i obrađuje

Stabilan ugovor omogućava komponentama da se menjaju bez nagađanja o značenju poruke.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 2.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Ugovor | oblik i značenje poruke | Šta se dešava kada ugovor nije dostupan ili daje zastarelu informaciju? |
| Izvor | šalje prema ugovoru | Šta se dešava kada izvor nije dostupan ili daje zastarelu informaciju? |
| Simulator | ponavlja poznat scenario | Šta se dešava kada simulator nije dostupan ili daje zastarelu informaciju? |
| Primalac | validira i tumači | Šta se dešava kada primalac nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 2.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Objavljena verzija ugovora čuva očekivanja postojećih korisnika.
2. Validacija odbija poruku koju sistem ne razume, umesto da joj nagađa značenje.
3. Simulator odvaja stvarni problem sistema od slučajnosti spoljnog sveta.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koju poruku biste najpre pretvorili u stabilan ugovor i koji scenario simulatora bi najbolje otkrio nesporazum između pošiljaoca i primaoca?

### 2.8. Izvršivi primer

Primer pokazuje dve stvari koje omogućavaju da se komponente razvijaju nezavisno: poruka ima eksplicitan ugovor koji se proverava na ulazu, a udaljena stanica se zamenjuje simulatorom čiji je tok u potpunosti određen scenarijom. Isti scenario uvek daje iste poruke, pa se i greška može ponoviti.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-02-ugovori-simulator
dotnet run --project src/Odp.Vezba02.ConsoleUi
dotnet test Odp.Vezba02.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-02-ugovori-simulator/src/Odp.Vezba02.Domain/Contracts/TelemetryContractValidator.cs

```csharp
public Result Validate(TelemetryMessage message)
{
    ArgumentNullException.ThrowIfNull(message);

    if (message.ContractVersion != TelemetryContract.Version)
        return Result.Fail(ContractErrorCodes.UnsupportedVersion);
    if (string.IsNullOrWhiteSpace(message.StationId))
        return Result.Fail(ContractErrorCodes.StationIdRequired);
    if (message.Sequence <= 0)
        return Result.Fail(ContractErrorCodes.SequenceMustBePositive);
    if (message.SignalStrengthDbm is < TelemetryContract.MinSignalDbm or > TelemetryContract.MaxSignalDbm)
        return Result.Fail(ContractErrorCodes.SignalOutOfRange);

    return Result.Ok();
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-02-ugovori-simulator/tests/Odp.Vezba02.Tests/Infrastructure/ScriptedStationSimulatorTests.cs

```csharp
[Test]
public void Emit_WithTheSameScenario_ProducesTheSameMessages()
{
    var first = new ScriptedStationSimulator(Scenario).Emit(20);
    var second = new ScriptedStationSimulator(Scenario).Emit(20);

    Assert.That(second, Is.EqualTo(first));
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba02.ConsoleUi

```text
Poruke simulatora:
  #1 09:00:00  -83.0 dBm
  #2 09:00:05 -105.0 dBm
  #3 09:00:10 -105.0 dBm
  #4 09:00:15    5.0 dBm
  #5 09:00:20 -104.0 dBm
  #6 09:00:25 -100.0 dBm
  #7 09:00:30  -81.0 dBm
  #8 09:00:35    5.0 dBm

Prihvaćeno: 6, odbijeno: 2
  #4 -> signal_out_of_range
  #8 -> signal_out_of_range

Ponovljeno pokretanje daje isti ishod: True
```

#### Šta treba uočiti

- Ugovor se proverava na ulazu, a svako odbijanje ima stabilan kod.
- Isti scenario (isti `Seed`) daje iste poruke, pa se i greška može ponoviti.
- Simulator je adapter iza porta `IStationSimulator`; pravila ne znaju da stanica nije prava.

> **Zadatak uz primer**
>
> Dodati u scenario kašnjenje poruke (`DelayEvery`), tako da svaka N-ta poruka nosi `MeasuredAt` stariji od prethodne. Uvesti pravilo ugovora koje takvu poruku odbija novim kodom i pokriti ga testom.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Ugovori, simulatori i ponovljivost". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P1 — Osnovni entiteti, ugovori i simulator

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.

## Vežba 3 — Identitet, audit, observability i konfiguracija

Distribuirani sistem mora moći da odgovori na četiri pitanja: ko je pokrenuo radnju, šta se dogodilo, kroz koje komponente je tok prošao i pod kojim uslovima je radio. Identitet, audit, observability i konfiguracija daju odgovore koji se ne smeju izmišljati iznova u svakoj funkcionalnosti.

Kada se tok pokvari, pojedinačni logovi nisu dovoljni. Zajednički korelacioni identifikator i jasan audit trag omogućavaju da tim sastavi priču o operaciji bez nagađanja i bez izlaganja osetljivih podataka.

### Poprečne mogućnosti koje svaka komponenta koristi na isti način

- Identitet: ko pokreće radnju
- Korelacija: jedan tok kroz komponente
- Audit: poslovno značajna odluka
- Konfiguracija: uslovi rada sistema

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 3.1. Vidljivost mora biti ugrađena

- Audit čuva odluke koje su važne za odgovornost i istragu.
- Logovi i tragovi čuvaju tehnički kontekst, ali ne tajne.
- Konfiguracija ima validno značenje, vlasnika i način promene.

> **Pitanje za diskusiju**
>
> Kako biste rekonstruisali jednu neuspelu operaciju koja je prošla kroz API, red poruka i worker, a da ne čitate neograničenu količinu logova?

### 3.2. Kako se ova tema vidi u razvoju

Tim bira nekoliko operacija koje mora umeti da prati od zahteva do ishoda. Svaka komponenta prenosi zajednički kontekst, a testovi proveravaju da promena konfiguracije ili prekid toka ostavlja dovoljno informacija za razumevanje.

#### Česte greške koje treba prepoznati

- Log koji otkriva kredencijale ili lične podatke.
- Korelacioni identifikator kreiran u svakoj komponenti umesto prenet.
- Konfiguracija prihvaćena bez provere opsega i uticaja.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 3.3. Primer: zahtev prolazi kroz tri komponente

Korisnik pokreće operaciju kroz API, API objavljuje poruku, a worker završava obradu. Ako svaka komponenta beleži svoj nepovezan identifikator, tim ne može brzo da odgovori da li je zahtev prihvaćen, obrađen ili izgubljen između koraka. Zajednički kontekst pretvara tri zasebna zapisa u jednu proverljivu priču.

- Korelacioni identifikator nastaje na granici toka i prenosi se dalje.
- Audit čuva odluke koje utiču na odgovornost, dok log čuva tehnički kontekst.
- Konfiguracija određuje očekivano ponašanje, kao što su rokovi i pragovi, i mora biti proverljiva.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 3.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 3.5. Šema sledljivog distribuiranog toka

### Šema sledljivog distribuiranog toka

- Ulazni zahtev: identitet i korelacija
- API: prihvata ili odbija
- Poruka i worker: obrađuju isti kontekst
- Audit i trag: objašnjavaju ishod

Jedan kontekst prolazi kroz sve komponente, pa se neuspeh može objasniti bez nagađanja.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 3.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Identitet | ko pokreće radnju | Šta se dešava kada identitet nije dostupan ili daje zastarelu informaciju? |
| Korelacija | jedan tok kroz komponente | Šta se dešava kada korelacija nije dostupan ili daje zastarelu informaciju? |
| Audit | poslovno značajna odluka | Šta se dešava kada audit nije dostupan ili daje zastarelu informaciju? |
| Konfiguracija | uslovi rada sistema | Šta se dešava kada konfiguracija nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 3.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Audit čuva odluke koje su važne za odgovornost i istragu.
2. Logovi i tragovi čuvaju tehnički kontekst, ali ne tajne.
3. Konfiguracija ima validno značenje, vlasnika i način promene.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Kako biste rekonstruisali jednu neuspelu operaciju koja je prošla kroz API, red poruka i worker, a da ne čitate neograničenu količinu logova?

### 3.8. Izvršivi primer

Primer prati jednu operaciju kroz dve komponente. Zakazivanje kontakta sa stanicom dobija identifikator operacije (`CorrelationId`) i identitet aktera; isti identifikator stiže do stanice i do revizijskog traga, pa se tok može rekonstruisati i kada delovi sistema vode odvojene logove. Ograničenje trajanja kontakta dolazi iz konfiguracije koja se proverava pre pokretanja.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-03-identitet-audit-konfiguracija
dotnet run --project src/Odp.Vezba03.ConsoleUi
dotnet test Odp.Vezba03.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-03-identitet-audit-konfiguracija/src/Odp.Vezba03.Application/Contacts/ScheduleContactHandler.cs

```csharp
public ScheduleContactResult Schedule(ContactRequest request, OperationContext context)
{
    ArgumentNullException.ThrowIfNull(request);
    ArgumentNullException.ThrowIfNull(context);

    var code = Decide(request, context);

    auditLog.Record(new AuditEntry(
        clock.UtcNow,
        context.CorrelationId,
        context.ActorId,
        Action,
        request.StationId,
        code));

    return new ScheduleContactResult(code == ContactCodes.Scheduled, code, context.CorrelationId);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-03-identitet-audit-konfiguracija/tests/Odp.Vezba03.Tests/Application/ScheduleContactHandlerTests.cs

```csharp
[Test]
public void Schedule_CarriesTheSameCorrelationIdToTheStationAndTheAuditTrail()
{
    var result = _handler.Schedule(Request, Context);

    using (Assert.EnterMultipleScope())
    {
        Assert.That(result, Is.EqualTo(new ScheduleContactResult(true, ContactCodes.Scheduled, "op-42")));
        Assert.That(_stationGateway.Calls.Single().CorrelationId, Is.EqualTo("op-42"));
        Assert.That(
            _auditLog.Entries.Single(),
            Is.EqualTo(new AuditEntry(
                Now,
                "op-42",
                "ana",
                "contact.schedule",
                "GS-NOVI-SAD",
                ContactCodes.Scheduled)));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba03.ConsoleUi

```text
op-0001: GS-NOVI-SAD, 10 min -> contact_scheduled
op-0002: GS-NOVI-SAD, 40 min -> duration_too_long
op-0003: GS-BEOGRAD, 10 min -> station_unavailable

Log stanice:
  op-0001 | GS-NOVI-SAD | reserved=True
  op-0003 | GS-BEOGRAD | reserved=False

Revizijski trag:
  op-0001 | ana | contact.schedule | GS-NOVI-SAD | contact_scheduled
  op-0002 | marko | contact.schedule | GS-NOVI-SAD | duration_too_long
  op-0003 | jelena | contact.schedule | GS-BEOGRAD | station_unavailable
```

#### Šta treba uočiti

- Isti `CorrelationId` vidi se u logu stanice i u revizijskom tragu centra.
- Revizijski zapis nastaje i kada je zahtev odbijen pre poziva stanice.
- Neispravna konfiguracija zaustavlja pokretanje sa kodom greške.

> **Zadatak uz primer**
>
> Dodati u `OperationContext` identifikator pozivajuće komponente (`Source`) i upisati ga u revizijski zapis. Proširiti test tako da dokazuje da zapis sadrži i izvor operacije.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Identitet, audit, observability i konfiguracija". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

## Vežba 4 — Verzionisanje i failure-first testiranje

U distribuiranom sistemu normalan scenario nikada nije cela priča. Poruka može da stigne dva puta, van redosleda ili nakon isteka roka. Verzije ugovora i kontrolisani failure scenariji omogućavaju timu da te situacije prouči dok su bezbedne i razumljive.

Failure-first pristup ne predviđa svaki mogući kvar. On podstiče tim da za važnu operaciju postavi prava pitanja: šta ako odgovor kasni, šta ako se zahtev ponovi i šta ako komponenta nestane između dve radnje?

### Razvoj koji računa na kašnjenje, duplikate i prekide

- Promena: novi ugovor ili ponašanje
- Kompatibilnost: šta stari korisnici razumeju
- Failure scenario: kontrolisana smetnja
- Test: dokaz očekivanog ishoda

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 4.1. Neizvesnost je deo ugovora

- Kompatibilnost se procenjuje prema primaocima poruke, ne samo prema kodu pošiljaoca.
- Kašnjenje i duplikat su normalni ulazi koje komponenta mora da objasni.
- Test treba da proveri vidljiv ishod, ne samo da metoda ne baca izuzetak.

> **Pitanje za diskusiju**
>
> Za koju operaciju bi kasno pristigla poruka bila opasnija od potpuno izgubljene poruke i zašto?

### 4.2. Kako se ova tema vidi u razvoju

Za najvažnije tokove tim formuliše nekoliko malih failure eksperimenata i pokreće ih u istom okruženju. Rezultat ulazi u testnu bazu i u opis odluke, pa sledeća promena ne mora ponovo da otkriva ista ograničenja.

#### Česte greške koje treba prepoznati

- Pretpostavka da se poruka isporučuje tačno jednom.
- Breaking promena označena kao bezopasna jer lokalni testovi prolaze.
- Testiranje prekida tek nakon integracije svih komponenti.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 4.3. Primer: potvrda stiže nakon isteka roka

Komponenta čeka odgovor do isteka roka i pokreće bezbedan oporavak. Zatim stigne stara potvrda iz prvog pokušaja. Ako sistem nema stabilan identitet operacije i pravilo za redosled, ta potvrda može prepisati novije stanje. Failure scenario zato nije izuzetak koji se samo uhvati, već deo ugovora o ponašanju.

- Timeout znači da odgovor nije stigao na vreme, ne da se radnja sigurno nije izvršila.
- Duplikat i kasna poruka moraju se razdvojiti od nove legitimne operacije.
- Ponovljiv simulator omogućava da se isti redosled greške proveri nakon svake izmene.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 4.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 4.5. Šema failure-first provere

### Šema failure-first provere

- Normalan tok: zahtev i očekivan odgovor
- Kontrolisana smetnja: kašnjenje, duplikat ili prekid
- Vidljiv ishod: stanje koje sistem zadržava
- Test: dokaz ponovljivog ponašanja

Cilj nije pretpostaviti savršen transport, već jasno definisati ponašanje kada transport nije savršen.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 4.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Promena | novi ugovor ili ponašanje | Šta se dešava kada promena nije dostupan ili daje zastarelu informaciju? |
| Kompatibilnost | šta stari korisnici razumeju | Šta se dešava kada kompatibilnost nije dostupan ili daje zastarelu informaciju? |
| Failure scenario | kontrolisana smetnja | Šta se dešava kada failure scenario nije dostupan ili daje zastarelu informaciju? |
| Test | dokaz očekivanog ishoda | Šta se dešava kada test nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 4.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Kompatibilnost se procenjuje prema primaocima poruke, ne samo prema kodu pošiljaoca.
2. Kašnjenje i duplikat su normalni ulazi koje komponenta mora da objasni.
3. Test treba da proveri vidljiv ishod, ne samo da metoda ne baca izuzetak.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Za koju operaciju bi kasno pristigla poruka bila opasnija od potpuno izgubljene poruke i zašto?

### 4.8. Izvršivi primer

Primer polazi od onoga što mreža sigurno radi: ista poruka može stići dva puta, može zakasniti i može biti poslata novijom verzijom ugovora. Prijemna strana zato za svaku poruku donosi eksplicitnu odluku, a testovi najpre pokrivaju neuspešne puteve.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-04-verzionisanje-failure-first
dotnet run --project src/Odp.Vezba04.ConsoleUi
dotnet test Odp.Vezba04.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-04-verzionisanje-failure-first/src/Odp.Vezba04.Domain/Messaging/InboxPolicy.cs

```csharp
public InboxDecision Decide(MessageEnvelope envelope, bool alreadyProcessed, DateTimeOffset now)
{
    ArgumentNullException.ThrowIfNull(envelope);

    if (envelope.Version.Major != rules.SupportedMajor)
        return new(false, InboxCodes.UnsupportedMajorVersion);
    if (alreadyProcessed)
        return new(false, InboxCodes.Duplicate);
    if (now - envelope.SentAt > rules.MaxAge)
        return new(false, InboxCodes.TooOld);

    return new(true, InboxCodes.Accepted);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-04-verzionisanje-failure-first/tests/Odp.Vezba04.Tests/Application/ReceiveMessageHandlerTests.cs

```csharp
[Test]
public void Receive_WhenTheSameMessageIsDeliveredTwice_ProcessesItOnce()
{
    var envelope = Envelope("m-1", 1, Start);

    var first = _handler.Receive(envelope);
    var second = _handler.Receive(envelope);

    using (Assert.EnterMultipleScope())
    {
        Assert.That(first.Code, Is.EqualTo(InboxCodes.Accepted));
        Assert.That(second.Code, Is.EqualTo(InboxCodes.Duplicate));
        Assert.That(_processor.ProcessedPayloads, Has.Count.EqualTo(1));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba04.ConsoleUi

```text
m-1 v1.0 (prva isporuka) -> message_accepted
m-1 v1.0 (ponovljena isporuka iste poruke) -> message_duplicate
m-2 v1.3 (novija minor verzija) -> message_accepted
m-3 v2.0 (druga major verzija) -> message_major_version_unsupported
m-4 v1.0 (poruka koja je putovala 12 minuta) -> message_too_old

Poslovni efekat se dogodio 2 puta:
  telemetrija m-1
  telemetrija m-2
```

#### Šta treba uočiti

- Duplikat, zakasnela poruka i druga major verzija su očekivani ulazi sa sopstvenim kodom.
- Poslovni efekat se dešava jednom, koliko god puta da ista poruka stigne.
- Novija minor verzija se prihvata; druga major verzija se odbija.

> **Zadatak uz primer**
>
> Dodati pravilo da se poruka sa minor verzijom većom od poznate prihvata, ali se ishod označava novim kodom `message_accepted_newer_minor`, kako bi tim znao da ugovor treba pregledati. Pokriti granični slučaj testom.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Verzionisanje i failure-first testiranje". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P2 — Identitet, audit, observability i konfiguracija

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.

## Vežba 5 — Tok podataka: prijem, validacija, routing i čitanje

Podatak koji stigne preko mreže još nije pouzdana poslovna informacija. Potrebno je utvrditi poreklo, validnost, kvalitet i način na koji će različiti delovi sistema dobiti svoj pogled na njega. Razdvajanje ovih odgovornosti čuva jasnoću kada se pojave kašnjenja ili duplikati.

Tok podataka obično služi više korisnika sa različitim potrebama. Routing odvaja njihovu obradu, a read modeli prilagođavaju podatak čitanju. Cena tog izbora je da prikaz može kasniti, što sistem treba otvoreno da pokaže.

### Od poruke sa mreže do informacije kojoj korisnik može da veruje

- Prijem: izvor i osnovna validacija
- Normalizacija: interni oblik i kvalitet
- Distribucija: nezavisni konzumenti
- Read model: pogled za korišćenje

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 5.1. Poreklo i kvalitet putuju sa podatkom

- Stabilan identitet poruke omogućava da se prepozna njeno ponavljanje.
- Vreme nastanka i vreme prijema imaju različito značenje.
- Read model pokazuje korisnu sliku, ali mora znati granicu svoje svežine.

> **Pitanje za diskusiju**
>
> Koji podatak u vašem sistemu može kasniti, a ipak ostati koristan? Kako korisnik treba da vidi tu ogradu?

### 5.2. Kako se ova tema vidi u razvoju

Tim opisuje koji sloj sme da odbije poruku, koji joj dodeljuje kvalitet i koji je čuva za kasnije čitanje. Ta podela olakšava testiranje jer svaka odluka ima svoj ulaz, vidljiv ishod i razlog u evidenciji.

#### Česte greške koje treba prepoznati

- Tiho odbacivanje poruke bez razloga i traga.
- Mešanje vremena izvora i prijema.
- Read model koji duplira poslovni efekat pri ponovljenoj poruci.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 5.3. Primer: poruka stiže dva puta i kasno

Telemetrijska poruka može stići dva puta, a njeno vreme nastanka može biti starije od vremena prijema. Ako skladište samo dodaje svaki zapis, prikaz poslednje vrednosti može postati pogrešan ili pretrpan duplikatima. Tok podataka zato čuva identitet poruke, poreklo i oba vremena dok ne formira read model za korisnika.

- Prijem proverava izvor i minimalni oblik poruke.
- Normalizacija čuva kvalitet i razliku između vremena nastanka i prijema.
- Read model otvoreno prikazuje svežinu informacije i ne pretvara duplikat u novi poslovni događaj.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 5.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 5.5. Šema toka podataka

### Šema toka podataka

- Prijem: izvor i identitet poruke
- Normalizacija: kvalitet i unutrašnji oblik
- Distribucija: nezavisna obrada
- Read model: pogled sa granicom svežine

Podatak postaje pouzdan tek kada su poznati njegovo poreklo, kvalitet i granice prikaza.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 5.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Prijem | izvor i osnovna validacija | Šta se dešava kada prijem nije dostupan ili daje zastarelu informaciju? |
| Normalizacija | interni oblik i kvalitet | Šta se dešava kada normalizacija nije dostupan ili daje zastarelu informaciju? |
| Distribucija | nezavisni konzumenti | Šta se dešava kada distribucija nije dostupan ili daje zastarelu informaciju? |
| Read model | pogled za korišćenje | Šta se dešava kada read model nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 5.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Stabilan identitet poruke omogućava da se prepozna njeno ponavljanje.
2. Vreme nastanka i vreme prijema imaju različito značenje.
3. Read model pokazuje korisnu sliku, ali mora znati granicu svoje svežine.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koji podatak u vašem sistemu može kasniti, a ipak ostati koristan? Kako korisnik treba da vidi tu ogradu?

### 5.8. Izvršivi primer

Primer razdvaja zapis od prikaza. Svako prihvaćeno merenje ulazi u zapis redom kojim je stiglo, a prikaz stanice se izvodi iz zapisa i pamti samo poslednje merenje. Merenje koje zakasni ostaje u zapisu, ali ne vraća prikaz unazad; prikaz koji dugo nije osvežen se označava kao zastareo umesto da izgleda kao trenutno stanje.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-05-tok-podataka-read-model
dotnet run --project src/Odp.Vezba05.ConsoleUi
dotnet test Odp.Vezba05.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-05-tok-podataka-read-model/src/Odp.Vezba05.Domain/ReadModels/StationViewProjector.cs

```csharp
public (StationView View, bool Changed) Apply(StationView? current, TelemetryReading reading)
{
    ArgumentNullException.ThrowIfNull(reading);

    if (current is not null && reading.Sequence <= current.LastSequence)
        return (current, false);

    return (
        new StationView(
            reading.StationId,
            reading.Sequence,
            reading.MeasuredAt,
            reading.SignalStrengthDbm),
        true);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-05-tok-podataka-read-model/tests/Odp.Vezba05.Tests/Application/IngestTelemetryHandlerTests.cs

```csharp
[Test]
public void Ingest_WhenOlderReadingArrivesLate_KeepsItInTheLogButNotInTheView()
{
    _handler.Ingest(Reading(3, -88));

    var outcome = _handler.Ingest(Reading(2, -97));

    using (Assert.EnterMultipleScope())
    {
        Assert.That(outcome, Is.EqualTo(new IngestOutcome(true, TelemetryCodes.Stored, ViewChanged: false)));
        Assert.That(_log.ForStation("GS-NOVI-SAD"), Has.Count.EqualTo(2));
        Assert.That(_views.Find("GS-NOVI-SAD")!.LastSequence, Is.EqualTo(3));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba05.ConsoleUi

```text
merenje #1 -> reading_stored, prikaz promenjen: True
merenje #3 -> reading_stored, prikaz promenjen: True
merenje #2 -> reading_stored, prikaz promenjen: False
merenje #0 -> sequence_must_be_positive, prikaz promenjen: False
[posle prijema] #3 -88 dBm, staro 5 s, zastarelo: False
[dva minuta bez novih merenja] #3 -88 dBm, staro 125 s, zastarelo: True

Zapis sadrži 3 merenja, prikaz samo poslednje.
```

#### Šta treba uočiti

- Zapis čuva sva prihvaćena merenja; prikaz čuva samo poslednje.
- Merenje koje zakasni ostaje u zapisu, ali ne menja prikaz.
- Prikaz koji dugo nije osvežen ostaje vidljiv, ali je označen kao zastareo.

> **Zadatak uz primer**
>
> Dodati use-case `RebuildStationView` koji briše prikaz stanice i ponovo ga izvodi iz zapisa. Testom pokazati da je rezultat isti bez obzira na redosled kojim su merenja prvobitno stigla.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Tok podataka: prijem, validacija, routing i čitanje". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

## Vežba 6 — Komande, poslovi, retry i idempotentnost

Komanda je zahtev za promenu sveta, a ne običan poziv metode. Nakon slanja, sistem može čekati, ponavljati pokušaj ili dobiti kasnu potvrdu. Zato se životni ciklus komande modeluje eksplicitno i razlikuje od lokalne namere korisnika.

Retry povećava verovatnoću uspeha, ali bez idempotentnosti može ponoviti poslovni efekat. Lease i ograničeno vlasništvo nad poslom sprečavaju da dva workera istovremeno dovrše istu radnju nakon neusaglašenog pogleda na stanje.

### Kako sistem napreduje kada potvrda nije trenutna ni sigurna

- Zahtev: validirana namera
- Izvršenje: rad u pozadini
- Potvrda: može kasniti
- Ishod: jednom zabeležen rezultat

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 6.1. Ponovni pokušaj ne sme menjati značenje radnje

- Status komande opisuje znanje sistema, ne nagađanje o udaljenom svetu.
- Idempotency ključ vezuje ponovljene pokušaje za isti poslovni rezultat.
- Lease ima rok i štiti sistem od zakašnjelog rada prethodnog vlasnika.

> **Pitanje za diskusiju**
>
> Koja bi posledica duplog izvršenja bila neprihvatljiva i koji identitet operacije sprečava takav ishod?

### 6.2. Kako se ova tema vidi u razvoju

Tim razdvaja prijem zahteva, obradu u pozadini i prikaz ishoda. Zatim testira prekid između njih: ponovljeni zahtev, pad workera i kasna potvrda ne smeju proizvesti novu poslovnu radnju niti poništiti novije stanje.

#### Česte greške koje treba prepoznati

- Retry bez granice i bez razloga za prestanak.
- Dve obrade koje obe veruju da su vlasnici posla.
- Kasna potvrda koja prepisuje završeno stanje.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 6.3. Primer: ponovljen zahtev za istu radnju

Klijent ne dobija odgovor na vreme i šalje isti zahtev ponovo. Sistem želi da bude otporan na prekid, pa drugi zahtev ne sme automatski smatrati novom namerom. Idempotency ključ povezuje ponovljene pokušaje sa istim poslovnim rezultatom, dok status životnog ciklusa jasno odvaja prihvatanje, obradu, čekanje i završetak.

- Ponovni pokušaj koristi isti identitet operacije kada namera korisnika nije promenjena.
- Worker koji preuzima posao ima ograničeno vreme vlasništva i ne može završiti rad nakon isteka.
- Kasna potvrda ne sme prepisati novije, završeno stanje.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 6.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 6.5. Šema pouzdane komande

### Šema pouzdane komande

- Zahtev: jedinstvena namera
- Red ili posao: obrada u pozadini
- Lease: privremeno vlasništvo workera
- Ishod: jednom zabeležen rezultat

Retry povećava šansu da se radnja završi, dok idempotentnost čuva njeno značenje.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 6.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Zahtev | validirana namera | Šta se dešava kada zahtev nije dostupan ili daje zastarelu informaciju? |
| Izvršenje | rad u pozadini | Šta se dešava kada izvršenje nije dostupan ili daje zastarelu informaciju? |
| Potvrda | može kasniti | Šta se dešava kada potvrda nije dostupan ili daje zastarelu informaciju? |
| Ishod | jednom zabeležen rezultat | Šta se dešava kada ishod nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 6.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Status komande opisuje znanje sistema, ne nagađanje o udaljenom svetu.
2. Idempotency ključ vezuje ponovljene pokušaje za isti poslovni rezultat.
3. Lease ima rok i štiti sistem od zakašnjelog rada prethodnog vlasnika.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koja bi posledica duplog izvršenja bila neprihvatljiva i koji identitet operacije sprečava takav ishod?

### 6.8. Izvršivi primer

Primer vodi komandu ka udaljenom uređaju kroz neizvesnost. Slanje nema povratnu vrednost: potvrda stiže odvojeno, kasni ili ne stigne. Zato komanda ima stabilan identifikator, ponovno slanje nosi isti identifikator, uređaj ga izvršava jednom, a potvrda koja stigne posle isteka i dalje ispravlja stanje.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-06-komande-idempotentnost
dotnet run --project src/Odp.Vezba06.ConsoleUi
dotnet test Odp.Vezba06.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-06-komande-idempotentnost/src/Odp.Vezba06.Domain/Commands/CommandLifecycle.cs

```csharp
public (DeviceCommand Command, string Code) Acknowledge(DeviceCommand command) =>
    command.State switch
    {
        CommandState.Sent =>
            (command with { State = CommandState.Acknowledged }, CommandCodes.Acknowledged),
        CommandState.TimedOut =>
            (command with { State = CommandState.Acknowledged }, CommandCodes.LateAckReconciled),
        _ => (command, CommandCodes.AckDuplicate)
    };
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-06-komande-idempotentnost/tests/Odp.Vezba06.Tests/Application/CommandFlowTests.cs

```csharp
[Test]
public void Run_WhenAckIsMissing_ResendsTheSameCommandAndTheDeviceExecutesItOnce()
{
    _dispatch.Dispatch("cmd-1", "antena-1", "rotate:120");
    _clock.Advance(TimeSpan.FromSeconds(12));

    var outcomes = _retry.Run();

    using (Assert.EnterMultipleScope())
    {
        Assert.That(outcomes, Is.EqualTo(new[] { new RetryOutcome("cmd-1", CommandCodes.Retried) }));
        Assert.That(_device.Deliveries, Is.EqualTo(2));
        Assert.That(_device.ExecutedCommandIds, Is.EqualTo(new[] { "cmd-1" }));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba06.ConsoleUi

```text
operator šalje komandu -> command_sent (pokušaj 1)
operator ponavlja isti zahtev -> command_already_dispatched (pokušaj 1)
potvrda nije stigla 12 s -> cmd-7: command_retried
potvrda nije stigla još 12 s -> cmd-7: attempts_exhausted
zakasnela potvrda -> late_ack_reconciled
ista potvrda ponovo -> ack_duplicate

Uređaj je primio 2 isporuke, a izvršio 1 komandu.
```

#### Šta treba uočiti

- Ponovljeni zahtev sa istim `CommandId` ne šalje komandu drugi put.
- Posle isteka se šalje ista komanda; uređaj je prima dva puta, a izvršava jednom.
- Potvrda koja stigne posle odustajanja ispravlja stanje (`late_ack_reconciled`).

> **Zadatak uz primer**
>
> Dodati odlaganje između pokušaja (`RetryDelay`) koje raste sa brojem pokušaja. Pravilo dodati u `CommandLifecycle` i testom pokazati da se ponovno slanje ne dešava pre isteka odlaganja.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Komande, poslovi, retry i idempotentnost". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P3 — Testiranje i manual-core-baseline

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.

## Vežba 7 — Rad bez veze i pouzdana isporuka

Prekid veze ne mora da zaustavi lokalni rad, ali uvodi pitanje šta se desilo dok druga strana nije bila dostupna. Buffer, replay i outbox obrasci čuvaju nameru i istoriju, dok deduplikacija sprečava da ponovna isporuka stvori drugi poslovni efekat.

Pouzdana isporuka nije obećanje da mreža nikada neće pogrešiti. Ona je skup jasnih odgovornosti koji omogućava da se poruka sačuva, ponovo pošalje, objasni ako ne prođe i obradi jednom sa stanovišta poslovnog efekta.

### Sistem koji čuva smisao poruke kroz prekid i ponovni kontakt

- Lokalna namera: nastaje i čuva se
- Outbox ili buffer: čeka sigurnu isporuku
- Primalac: pamti obrađene poruke
- DLQ i replay: vidljiva korekcija neuspeha

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 7.1. Isporuka i obrada su odvojene odgovornosti

- Poruka koja ne prolazi ne nestaje tiho, već dobija vidljiv razlog.
- Ponovljena isporuka je očekivana i primalac je prepoznaje.
- Lokalni kapacitet i politika odbacivanja moraju biti eksplicitni.

> **Pitanje za diskusiju**
>
> Koju poruku biste morali sačuvati tokom prekida veze, a koju biste smeli izgubiti uz jasno obaveštenje? Zašto?

### 7.2. Kako se ova tema vidi u razvoju

Tim najpre odlučuje šta može da se zadrži lokalno, koliko dugo i šta se dešava pri popunjenom kapacitetu. Zatim proverava reconnect scenarije i pokazuje da se poruka može pratiti od lokalne namere do konačnog poslovnog ishoda.

#### Česte greške koje treba prepoznati

- Buffer bez ograničenja i bez politike pri popunjenosti.
- Replay koji zaobilazi istu validaciju kao prvi pokušaj.
- Deduplikacija oslonjena samo na podatak koji se može promeniti.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 7.3. Primer: reconnect posle lokalnog rada

Udaljena komponenta nastavlja lokalni rad dok nema vezu i čuva poruke za kasnije. Kada se ponovo poveže, neke od tih poruka su možda već stigle drugim putem. Pouzdana isporuka zato ne znači da se sve šalje bez provere, već da se poruke mogu identifikovati, ponoviti i obrađivati bez duplog poslovnog efekta.

- Buffer ima ograničenje i jasno pravilo kada više nema prostora.
- Outbox čuva nameru zajedno sa poslovnom promenom koju predstavlja.
- Primalac vodi evidenciju obrađenih poruka, a dead-letter čuva neuspehe za kontrolisan replay.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 7.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 7.5. Šema rada bez veze

### Šema rada bez veze

- Lokalna promena: nastaje bez mreže
- Buffer ili outbox: čuva poruku i razlog
- Reconnect: ponavlja isporuku
- Inbox i rezultat: sprečavaju dupli efekat

Prekid veze menja trenutak isporuke, ali ne sme da izbriše nameru niti da je proizvoljno udvostruči.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 7.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Lokalna namera | nastaje i čuva se | Šta se dešava kada lokalna namera nije dostupan ili daje zastarelu informaciju? |
| Outbox ili buffer | čeka sigurnu isporuku | Šta se dešava kada outbox ili buffer nije dostupan ili daje zastarelu informaciju? |
| Primalac | pamti obrađene poruke | Šta se dešava kada primalac nije dostupan ili daje zastarelu informaciju? |
| DLQ i replay | vidljiva korekcija neuspeha | Šta se dešava kada dlq i replay nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 7.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Poruka koja ne prolazi ne nestaje tiho, već dobija vidljiv razlog.
2. Ponovljena isporuka je očekivana i primalac je prepoznaje.
3. Lokalni kapacitet i politika odbacivanja moraju biti eksplicitni.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Koju poruku biste morali sačuvati tokom prekida veze, a koju biste smeli izgubiti uz jasno obaveštenje? Zašto?

### 7.8. Izvršivi primer

Primer čuva nameru kroz prekid veze. Stanica svako merenje najpre upisuje u lokalni outbox, a slanje je odvojen korak koji se ponavlja dok ne stigne potvrda. Pošto potvrda može da se izgubi i posle uspešne isporuke, ista poruka ponekad stigne dva puta; centar je zato primenjuje samo prvi put.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-07-rad-bez-veze-outbox
dotnet run --project src/Odp.Vezba07.ConsoleUi
dotnet test Odp.Vezba07.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-07-rad-bez-veze-outbox/src/Odp.Vezba07.Application/Flushing/FlushOutboxHandler.cs

```csharp
public FlushReport Flush()
{
    var pending = ordering.PendingInOrder(outbox.All());
    if (pending.Count == 0)
        return new(0, 0, OutboxCodes.NothingPending);

    var delivered = 0;
    foreach (var message in pending)
    {
        if (!uplink.TrySend(message))
            return new(delivered, pending.Count - delivered, OutboxCodes.DeliveryUnconfirmed);

        outbox.MarkDelivered(message.Position);
        delivered++;
    }

    return new(delivered, 0, OutboxCodes.Flushed);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-07-rad-bez-veze-outbox/tests/Odp.Vezba07.Tests/Application/OutboxFlowTests.cs

```csharp
[Test]
public void Flush_WhenAckIsLost_ResendsAndTheCenterAppliesTheMessageOnce()
{
    _record.Record("m-1", "a");
    _record.Record("m-2", "b");
    _uplink.LoseNextAck = true;

    var first = _flush.Flush();
    var second = _flush.Flush();

    using (Assert.EnterMultipleScope())
    {
        Assert.That(first, Is.EqualTo(new FlushReport(0, 2, OutboxCodes.DeliveryUnconfirmed)));
        Assert.That(second, Is.EqualTo(new FlushReport(2, 0, OutboxCodes.Flushed)));
        Assert.That(_center.AppliedPayloads, Is.EqualTo(new[] { "a", "b" }));
        Assert.That(_center.DuplicatesIgnored, Is.EqualTo(1));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba07.ConsoleUi

```text
Veza je u prekidu.
  merenje m-1 -> message_queued
  merenje m-2 -> message_queued
  merenje m-3 -> message_queued
  pokušaj slanja bez veze -> delivery_unconfirmed, isporučeno 0, preostalo 3
Veza se vratila, ali se prva potvrda gubi.
  prvi pokušaj posle povratka -> delivery_unconfirmed, isporučeno 0, preostalo 3
  drugi pokušaj -> outbox_flushed, isporučeno 3, preostalo 0

Centar je primenio 3 poruke redom:
  signal -91 dBm
  signal -88 dBm
  signal -97 dBm
Ignorisanih duplikata: 1
```

#### Šta treba uočiti

- Upis merenja uspeva i kada veze nema; slanje je odvojen korak.
- Posle povratka veze poruke stižu redom kojim su nastale.
- Izgubljena potvrda izaziva ponovno slanje, a centar poruku primenjuje jednom.

> **Zadatak uz primer**
>
> Ograničiti veličinu outbox-a (`Capacity`). Kada je pun, novo merenje se odbija kodom `outbox_full` umesto da se tiho izgubi staro. Pravilo pokriti testom u kome veza ostaje u prekidu.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Rad bez veze i pouzdana isporuka". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

## Vežba 8 — Koordinacija, protok i eventualna konzistentnost

Napredni distribuirani dizajn prihvata da različite komponente mogu privremeno imati različit pogled na stanje. Izazov nije da se ta činjenica sakrije, već da se vlasništvo, prebacivanje rada, opterećenje i svežina podataka modeluju tako da korisnik i operater znaju šta sistem može da garantuje.

Koordinacija sprečava sukob nad ekskluzivnim radom, backpressure štiti sistem kada obrada zaostaje, a replike ubrzavaju čitanje uz cenu privremene zastarelosti. Svaki mehanizam ima korist i cenu koju tim mora umeti da objasni.

### Svesni kompromisi kada sistem raste i radi pod pritiskom

- Vlasništvo: jedan važeći izvršilac
- Opterećenje: ritam proizvodnje i obrade
- Replika: brže čitanje uz kašnjenje
- Jasan ugovor: vidljiva ograničenja

Distribuirana svojstva nastaju u odnosu između komponenti, vremena i granica odgovornosti.

> **Fokus vežbe**
>
> Materijal objašnjava distribuirani problem, posledice izbora i granice garancije koju sistem može da pruži.

### 8.1. Konzistentnost je odluka o garanciji

- Fencing token sprečava da stari vlasnik nastavi rad nakon preuzimanja.
- Backpressure bira kontrolisano usporavanje ili ograničen gubitak pre potpunog kolapsa.
- Read model može biti zastareo, ali sistem mora pokazati koliko i kada to utiče na odluku.

> **Pitanje za diskusiju**
>
> Za jednu odluku navedite da li zahteva najnovije stanje ili može da koristi zastareo prikaz, pa objasnite posledicu izbora.

### 8.2. Kako se ova tema vidi u razvoju

Završna odbrana ne traži savršenu arhitekturu. Traži jasan trade-off: koje stanje mora biti trenutno, gde je prihvatljivo kašnjenje, kako se sprečava stari vlasnik i šta sistem radi kada ne može da prati ulazni ritam.

#### Česte greške koje treba prepoznati

- Failover pokrenut na osnovu jednog kratkog prekida.
- Neograničen red poruka koji samo odlaže kvar.
- Korišćenje zastarelog read modela kao da je autoritativno stanje.

> **Veza sa ostatkom gradiva**
>
> Ovaj princip se nadovezuje na ugovore, failure scenarije, observability i testiranje. Ista logika važi bez obzira na konkretnu tehnologiju ili domen sistema.

### 8.3. Primer: dva čvora i jedan ekskluzivan posao

Dve instance nakratko izgube vezu jedna sa drugom. Obe mogu misliti da smeju da nastave isti ekskluzivan posao. Fencing token ili epoha daje novom vlasniku jači dokaz vlasništva, pa sistem može odbiti zakašneli pokušaj stare instance čak i kada se ona kasnije ponovo pojavi.

- Koordinacija bira jednog važećeg izvršioca za rad koji ne sme biti paralelan.
- Backpressure odlučuje kako sistem reaguje kada ulazni ritam nadmaši obradu.
- Replika ubrzava čitanje, ali njen prikaz mora moći da pokaže zastarelost.

> **Kako se scenario analizira**
>
> Najpre razdvojite ono što sistem zna od onoga što pretpostavlja. Zatim odredite stabilan identitet operacije i vidljiv ishod koji se može proveriti.

### 8.4. Od principa do dokaza

Distribuirani princip je koristan tek kada sistem ima jasan, proverljiv odgovor na njegov failure scenario. Dobar dokaz ne pokazuje samo da je normalan tok uspeo, već i da kašnjenje, duplikat, prekid ili kasna poruka ne menjaju poslovno značenje operacije.

- Imenovati stanje koje je autoritativno i komponentu koja ima pravo da ga menja.
- Navesti šta se dešava kada odgovor izostane ili stigne nakon isteka roka.
- Ponavljati kontrolisani scenario dok se ishod može objasniti i nezavisno proveriti.

### 8.5. Šema rada pod pritiskom

### Šema rada pod pritiskom

- Vlasništvo: jedan važeći izvršilac
- Opterećenje: kontrolisan ritam obrade
- Replika: brže čitanje uz kašnjenje
- Jasan ugovor: vidljive granice garancije

Napredni distribuirani dizajn ne skriva kompromis, već korisniku i operateru jasno pokazuje garanciju.

Šemu koristite kao kratku proveru: za svaki korak treba umeti navesti vlasnika odluke, poruku ili stanje koje se prenosi i failure scenario koji može promeniti tok.

### 8.6. Uporedni pregled elemenata toka

| Element | Uloga u toku | Pitanje koje treba postaviti |
| --- | --- | --- |
| Vlasništvo | jedan važeći izvršilac | Šta se dešava kada vlasništvo nije dostupan ili daje zastarelu informaciju? |
| Opterećenje | ritam proizvodnje i obrade | Šta se dešava kada opterećenje nije dostupan ili daje zastarelu informaciju? |
| Replika | brže čitanje uz kašnjenje | Šta se dešava kada replika nije dostupan ili daje zastarelu informaciju? |
| Jasan ugovor | vidljiva ograničenja | Šta se dešava kada jasan ugovor nije dostupan ili daje zastarelu informaciju? |

> **Poređenje pojmova**
>
> Svaki element toka rešava drugi deo problema. Stabilan dizajn ne prebacuje odgovornost jednog elementa na drugi samo zato što je to kratkoročno jednostavnije.

### 8.7. Pitanja za analizu distribuiranog ponašanja

Kada se analizira distribuirani tok, nije dovoljno opisati samo srećan put. Potrebno je imenovati stanje koje se menja, granicu odgovornosti, poruku ili vreme koje može zakasniti i ishod koji sistem mora sačuvati uprkos neizvesnosti.

1. Fencing token sprečava da stari vlasnik nastavi rad nakon preuzimanja.
2. Backpressure bira kontrolisano usporavanje ili ograničen gubitak pre potpunog kolapsa.
3. Read model može biti zastareo, ali sistem mora pokazati koliko i kada to utiče na odluku.
4. Koji kontrolisani scenario bi pokazao da se poslovno značenje operacije ne menja pri kašnjenju, duplikatu ili prekidu?

> **Pitanje za vežbu**
>
> Za jednu odluku navedite da li zahteva najnovije stanje ili može da koristi zastareo prikaz, pa objasnite posledicu izbora.

### 8.8. Izvršivi primer

Primer pokazuje dve garancije koje sistem mora da izabere kada raste. Prva je vlasništvo: samo jedan worker sme da menja raspored stanice, vlasništvo je vremenski ograničeno (lease), a resurs odbija upis bivšeg vlasnika po zastarelom tokenu. Druga je protok: kada je red pun, pošiljalac dobija jasan odgovor umesto da posao neograničeno čeka.

Pokretanje demonstracije i testova

```bash
cd examples/odp-dotnet/vezba-08-koordinacija-protok
dotnet run --project src/Odp.Vezba08.ConsoleUi
dotnet test Odp.Vezba08.sln
```

#### Pravilo u kodu

examples/odp-dotnet/vezba-08-koordinacija-protok/src/Odp.Vezba08.Domain/Leases/LeasePolicy.cs

```csharp
public LeaseDecision Acquire(Lease? current, string resource, string candidateId, DateTimeOffset now)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(resource);
    ArgumentException.ThrowIfNullOrWhiteSpace(candidateId);

    if (current is null || now >= current.ExpiresAt)
    {
        var token = (current?.FencingToken ?? 0) + 1;
        return new(true, LeaseCodes.Granted, new Lease(resource, candidateId, token, now + _duration));
    }

    return current.OwnerId == candidateId
        ? new(true, LeaseCodes.Renewed, current with { ExpiresAt = now + _duration })
        : new(false, LeaseCodes.HeldByAnother, current);
}
```

#### Test koji zadaje kvar

examples/odp-dotnet/vezba-08-koordinacija-protok/tests/Odp.Vezba08.Tests/Application/CoordinationFlowTests.cs

```csharp
[Test]
public void Write_WhenFormerOwnerWakesUpAfterTakeover_IsRejectedByItsStaleToken()
{
    var former = _acquire.Acquire(Resource, "worker-a").Lease;
    _clock.Advance(TimeSpan.FromSeconds(45));
    var current = _acquire.Acquire(Resource, "worker-b").Lease;
    _write.Write(Resource, current.FencingToken, "plan B");

    var code = _write.Write(Resource, former.FencingToken, "zakasneli plan A");

    using (Assert.EnterMultipleScope())
    {
        Assert.That(code, Is.EqualTo(FencingCodes.StaleToken));
        Assert.That(_schedules.ValueOf(Resource), Is.EqualTo("plan B"));
    }
}
```

#### Ispis demonstracije

dotnet run --project src/Odp.Vezba08.ConsoleUi

```text
Koordinacija:
  worker-a traži lease -> lease_granted (vlasnik worker-a, token 1)
  worker-b traži lease -> lease_held_by_another (vlasnik worker-a, token 1)
  worker-a upisuje "plan A" sa tokenom 1 -> write_accepted
  worker-a je zastao 45 s; lease je istekao
  worker-b traži lease -> lease_granted (vlasnik worker-b, token 2)
  worker-b upisuje "plan B" sa tokenom 2 -> write_accepted
  worker-a upisuje "zakasneli plan A" sa tokenom 1 -> stale_fencing_token
  važeći raspored: plan B

Protok (kapacitet reda 2):
  job-1 -> job_accepted
  job-2 -> job_accepted
  job-3 -> queue_overloaded
```

#### Šta treba uočiti

- U jednom trenutku samo jedan worker je vlasnik resursa.
- Posle isteka lease-a novi vlasnik dobija veći token, a upis bivšeg vlasnika je odbijen.
- Kada je red pun, pošiljalac dobija `queue_overloaded` umesto tihog čekanja.

> **Zadatak uz primer**
>
> Dodati u prikaz rasporeda podatak o svežini: vreme poslednjeg upisa i oznaku `IsStale` kada je prikaz stariji od zadatog praga. Testom pokazati da čitalac vidi zastareo, ali jasno označen podatak dok vlasnik ne upiše novi.

> **Rad na vežbi — primena na projekat**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Koordinacija, protok i eventualna konzistentnost". Identifikovati konkretnu distribuiranu odluku, implementirati je i pripremiti ponovljiv failure scenario i test koji tim može da pokaže na projektnoj kontrolnoj tački.

### P4 — Operativni i napredni nivo — završna odbrana

> **Kontrolna tačka**
>
> Ova tačka zaokružuje prethodne teme. Organizacioni detalji i zahtevi nalaze se u posebnom odeljku kontrolnih tačaka.

## Sažetak: distribuiran sistem kao objašnjiv, testiran proces

Kroz osam vežbi gradi se jedan konzistentan lanac: entitet i vlasništvo → ugovor → distribuirana operacija → failure/recovery scenario → test → evidencija. Taj lanac ostaje isti bez obzira na to da li se radi o osnovnom heartbeat mehanizmu, command dispatch-u ili naprednom regionalnom failover-u — menja se samo nivo sistema na kome se primenjuje.

| Nivo | Šta uvodi |
| --- | --- |
| R1 — osnovni | Misije, stanice, node-ovi, ugovori, identitet, audit, observability. |
| R2 — operativni | Telemetrija, komande, jobs, messaging, lease, DLQ, reconnect. |
| R3 — napredni | Coordination, failover, replikacija, backpressure, eventual consistency. |

> **Odgovornost ostaje kod studenta**
>
> Automatizacija i AI podrška mogu ubrzati implementaciju, ali tim mora razumeti distribuirani model, objasniti trade-off odabrane strategije i pokazati nezavisan dokaz da failure/recovery ponašanje zaista radi.

## Preporučena literatura i dokumentacija

Literatura služi za produbljivanje tema iz praktikuma. Preporuka je da se čita uz konkretan distribuirani scenario, jer se principi najbrže usvajaju kada student može da poveže definiciju sa failure scenarijem, testom ili observability zapisom.

- Martin Kleppmann — *Designing Data-Intensive Applications*.
- Chris Richardson — *Microservices Patterns* (Saga, Outbox, Transactional messaging).
- Google SRE Book — *Site Reliability Engineering*: [sre.google/books](https://sre.google/books/).
- Microsoft Learn — Cloud Design Patterns (Retry, Circuit Breaker, Competing Consumers): [learn.microsoft.com/azure/architecture/patterns](https://learn.microsoft.com/azure/architecture/patterns/).
- NUnit dokumentacija: [docs.nunit.org](https://docs.nunit.org); Moq dokumentacija: [github.com/devlooped/moq](https://github.com/devlooped/moq).
- Pat Helland — *Life beyond Distributed Transactions* (rad o idempotenciji i outbox obrascu).
- Leslie Lamport — *Time, Clocks, and the Ordering of Events in a Distributed System* (osnova za temu logičkog vremena i redosleda događaja).

> **Napomena o alatima**
>
> Konkretni message broker-i, orkestratori i cloud platforme menjaju se brže od osnovnih distribuiranih principa. Simulatori korišćeni u praktikumu oponašaju njihovo ponašanje radi vežbe — ne predstavljaju se kao realni proizvodi.
