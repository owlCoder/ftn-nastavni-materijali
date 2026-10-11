# Osnove distribuiranog programiranja — Praktikum

FTN · Nastavni materijali

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
