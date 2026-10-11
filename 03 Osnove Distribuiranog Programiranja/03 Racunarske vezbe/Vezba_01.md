# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

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
