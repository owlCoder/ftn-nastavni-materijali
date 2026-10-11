# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

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
