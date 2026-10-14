# Elementi razvoja softvera — Praktikum

FTN · Nastavni materijali

## Vežba 4 — Testabilni dizajn, NUnit, Moq i pokrivenost koda

Testabilni dizajn znači da se komponente mogu proveravati izolovano i deterministički. Razdvajanje odgovornosti, jasni ugovori i dependency injection omogućavaju da se promenljive zavisnosti kontrolišu u testu. Unit testiranje zato nije završna aktivnost koja se naknadno dodaje gotovom kodu, već povratna informacija o kvalitetu granica sistema.

![Testiranje i pokrivenost koda](https://owlcoder.github.io/ftn-nastavni-portal/course-assets/testing.svg)

Veza između testabilnog dizajna, test scenarija, test dvojnika i signala o pokrivenosti koda.

| Element | Uloga |
| --- | --- |
| NUnit | Framework za organizaciju i izvršavanje testova. |
| Microsoft.NET.Test.Sdk | Infrastruktura koja omogućava .NET test runner-u da pronađe i izvrši testove. |
| NUnit3TestAdapter | Adapter za integraciju NUnit testova sa IDE i test runner okruženjem. |
| Moq | Biblioteka za kreiranje test dvojnika i proveru saradnje sa zavisnostima. |
| Alat za pokrivenost koda | Signal koji pokazuje koje linije ili grane nisu izvršene testovima; ne predstavlja ocenu kvaliteta testova. |

### 4.1. Arrange – Act – Assert

Jedan unit test treba da ima jasan razlog postojanja. Arrange priprema ulaz i saradnike, Act izvršava jednu relevantnu operaciju, a Assert proverava posmatrani rezultat. Naziv testa treba da opisuje ponašanje koje štiti, a ne samo metodu koju poziva.

### Struktura jednog testa

- Arrange: ulazi i kontrolisani saradnici
- Act: jedna relevantna operacija
- Assert: rezultat ili značajna interakcija

Jasno razdvojene faze čine test lakšim za čitanje i dijagnostiku.

Negativni scenario izveden iz kriterijuma prihvatanja

```csharp
[Test]
public void Reserve_WhenPeriodOverlaps_ReturnsConflict()
{
    // Arrange
    var repository = new Mock<IReservationRepository>();
    repository
        .Setup(r => r.HasOverlap(
            It.IsAny<Guid>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>()))
        .Returns(true);

    var clock = new Mock<IClock>();
    clock.SetupGet(c => c.UtcNow)
        .Returns(new DateTime(2026, 10, 1));

    var service = new ReservationService(
        repository.Object, clock.Object);

    // Act
    var result = service.Reserve(
        TestRequests.ValidFutureReservation());

    // Assert
    Assert.That(result.Success, Is.False);
    Assert.That(result.Error, Is.EqualTo("OverlappingReservation"));
    repository.Verify(
        r => r.Add(It.IsAny<Reservation>()), Times.Never);
}
```

![Test Explorer i rezultat izvršavanja](https://owlcoder.github.io/ftn-nastavni-portal/course-assets/test-explorer.svg)

Test Explorer omogućava pregled uspešnih i neuspešnih testova, trajanja izvršavanja i poruke koja vodi ka uzroku neuspeha.

> **Test proverava ponašanje, a ne privatne detalje implementacije**
>
> Ako test prestaje da prolazi svaki put kada se privatna metoda preimenuje ili se kod refaktoriše bez promene spolja vidljivog ponašanja, test je verovatno previše vezan za implementacione detalje.

### 4.2. Moq i test dvojnici

Moq omogućava kreiranje kontrolisanih test dvojnika za zavisnosti koje test ne želi stvarno da pozove. Time se mogu simulirati različiti povratni ishodi, izuzeci ili značajne interakcije bez pokretanja realne baze podataka ili spoljnog servisa. Mock nije zamena za svaki stvarni objekat; koristi se na granici koju test namerno izoluje.

| Situacija | Da li koristiti mock? |
| --- | --- |
| Repozitorijum ili HTTP klijent koji test ne želi stvarno da pozove | Da — kontrolisati povratnu vrednost ili izuzetak. |
| `IClock` ili generator identifikatora kada test zavisi od vremena ili slučajnosti | Da — obezbediti deterministički test. |
| Mali nepromenljivi value object | Najčešće ne — koristiti stvarni objekat. |
| Domenska klasa čije ponašanje je predmet testa | Ne — testirati stvarni objekat. |

Setup kontroliše saradnika, a Verify proverava značajnu interakciju

```csharp
var repository = new Mock<IReservationRepository>();
repository
    .Setup(r => r.FindById(reservationId))
    .Returns(existingReservation);

var result = service.Cancel(reservationId);

repository.Verify(
    r => r.Save(existingReservation), Times.Once);
```

> **Veliki broj Verify provera može ukazivati na krhak test**
>
> Ako test proverava redosled i broj gotovo svakog internog poziva, lako postaje zavisan od detalja implementacije. Interakciju treba proveravati kada ona predstavlja deo ugovora ponašanja, na primer da se podatak ne čuva nakon neuspešne validacije.

### 4.3. Iz zahteva u plan testiranja

Najvredniji testovi nastaju iz poslovnih pravila i kriterijuma prihvatanja, a ne iz nasumičnog obilaska metoda. Za ključni use-case treba identifikovati uspešan tok, očekivane negativne ishode i granične vrednosti koje menjaju ponašanje.

| Vrsta scenarija | Primer za rezervaciju |
| --- | --- |
| Uspešan scenario | Oprema je slobodna, korisnik ovlašćen, a termin validan. |
| Negativni scenario | Termin se preklapa sa postojećom rezervacijom. |
| Negativni scenario | Oprema je označena kao neaktivna. |
| Granični scenario | Rezervacija počinje tačno u trenutku kada se prethodna završava — očekivano pravilo mora biti eksplicitno definisano. |
| Infrastrukturni kvar | Repozitorijum prijavljuje grešku; infrastrukturni kvar se ne predstavlja kao regularna poslovna odbijenica. |

> **Rad na vežbi**
>
> Za jedan projektni use-case napisati plan testiranja od najmanje šest scenarija pre pisanja test koda. Svaki scenario povezati sa pravilom ili kriterijumom prihvatanja koji štiti.

### 4.4. Pokrivenost koda kao signal, ne cilj

Pokrivenost koda pokazuje koji su delovi programa izvršeni tokom testova, ali ne govori da li su provere smisleno napisane niti da li su obuhvaćeni najvažniji rizici. Zato se izveštaj o pokrivenosti koristi kao dijagnostički signal za pronalaženje nepokrivenih grana, a ne kao samostalna ocena kvaliteta.

Pokretanje testova i prikupljanje podataka o pokrivenosti

```bash
dotnet test
# primer sa collector-om, ako je odgovarajuće podešavanje dostupno
dotnet test --collect:"XPlat Code Coverage"
```

![Primer interpretacije izveštaja o pokrivenosti koda](https://owlcoder.github.io/ftn-nastavni-portal/course-assets/coverage-report.svg)

Izveštaj o pokrivenosti treba tumačiti prema linijama i granama koje nose poslovni rizik, a ne samo prema ukupnom procentu.

- Najpre definisati poslovno važne scenarije, a tek zatim analizirati pokrivenost koda.
- Ne dodavati testove trivijalnih getter-a samo radi povećanja procenta ako važna grana ostaje neproverena.
- Posebno pregledati uslovne grane, obradu grešaka i kod koji menja novac, dozvole, rezervacije ili drugo kritično stanje.
- Niska pokrivenost može ukazivati na propust; visoka pokrivenost ne dokazuje odsustvo grešaka.

> **Pitanje za pregled**
>
> Koji deo nepokrivenog koda nosi najveći rizik i koji test bi dao najviše novih informacija? To je korisnije pitanje od zahteva da se dostigne unapred zadat procenat.

> **Mini domaći — bonus 2 boda**
>
> AI alatu dati opis use-case-a i postojeće testove, ali tražiti isključivo predlog nedostajućih scenarija. Prvi bod se dobija za novi smisleni granični test; drugi za obrazloženje zbog čega je predlog prihvaćen ili odbačen.

### Kontrolna tačka KT1 — Osnovna implementacija

Prva kontrolna tačka održava se u terminu 4. vežbe i proverava osnovnu implementaciju svih dodeljenih celina.

- Repozitorijum i pokretanje projekta; strukturiran Tapiz backlog i podela rada.
- Osnovni domenski model, poslovna pravila i API/contract granice za sve dodeljene celine.
- Najmanje jedan izvršiv funkcionalni tok i automatizovani test značajnog pravila.
- Dogovor sa zavisnim timovima ili kontrolisani stub kada zavisna komponenta nije spremna.

> **Termin i obuhvat**
>
> Vežba 1–4 · Vežba 4 · 10 poena. Isti zahtevi prikazani su na kartici „Kontrolne tačke“.
