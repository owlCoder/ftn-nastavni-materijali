# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

## Vežba 3 — Politike, konfiguracija i bezbednosna vidljivost

Bezbednosna pravila postaju pouzdana tek kada su vidljiva, verzionisana i proverljiva. Pravilo rasuto kroz nekoliko metoda nije lako objasniti, pregledati niti promeniti bez neželjenih posledica. Politika zato predstavlja dogovoreno pravilo, a konfiguracija njegovu primenu u konkretnom okruženju.

Vidljivost ne služi samo reagovanju na incident. Ona omogućava timu da razume odluku sistema, prati odstupanje od očekivanog stanja i poveže događaje kroz različite slojeve aplikacije.

### Eksplicitna pravila umesto bezbednosti skrivene u kodu

- Politika: šta organizacija očekuje
- Konfiguracija: kako se pravilo primenjuje
- Kontrola: mehanizam zaštite
- Dokaz: log, metrika ili test

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 3.1. Pravilo bez dokaza je samo namera

Vidljivost ne služi samo reagovanju na incident. Ona omogućava timu da razume odluku sistema, prati odstupanje od očekivanog stanja i poveže događaje kroz različite slojeve aplikacije.

- Nova verzija politike ne briše razlog zbog kog je stara odluka nekada bila ispravna.
- Konfiguraciona vrednost ima vlasnika, značenje i očekivani opseg.
- Korelacioni identifikator povezuje jedan bezbednosni tok kroz slojeve sistema.

> **Pitanje za diskusiju**
>
> Koje bezbednosno pravilo bi trebalo da bude zaseban, čitljiv artefakt u vašoj celini i koji dokaz pokazuje da radi?

### 3.2. Kako se ova tema vidi u razvoju

Tim bira nekoliko ključnih odluka koje mora moći da objasni iz loga i testa. To nisu svi događaji sistema, već oni čiji bi nejasan ishod otežao istragu, pregled pristupa ili korekciju konfiguracije.

#### Česte greške koje treba prepoznati

- Uređivanje objavljene politike na mestu bez istorije.
- Tajne u konfiguracionim datotekama ili logovima.
- Metrika bez jasnog pitanja na koje treba da odgovori.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 3.3. Primer: promena pravila bez gubitka istorije

Tim pooštrava pravilo za pristup administrativnom interfejsu. Ako staro pravilo samo prepiše novim, kasnije ne može da objasni zašto je raniji pristup bio dozvoljen niti da rekonstruiše koje je očekivanje tada važilo. Verzionisanje ne služi birokratiji, već očuvanju smisla istorijskih odluka.

- Politika opisuje nameru i važenje, dok konfiguracija čuva vrednosti kojima se ta namera primenjuje u okruženju.
- Svaka promena koja utiče na odluku ima vlasnika, datum i jasan razlog.
- Log, metrika ili test daju dokaz da sistem radi prema aktivnom pravilu, a ne samo da je pravilo zapisano.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 3.4. Dizajnerska odluka: pravilo mora imati granice

Politika postaje korisna kada je dovoljno precizna da ljudi donose dosledne odluke, a dovoljno čitljiva da se može objasniti bez čitanja izvornog koda. Dobro pravilo navodi na šta se odnosi, ko je vlasnik, kada važi i šta se dešava ako se pravila sukobe.

- Konflikt politika rešava se unapred dogovorenim pravilom, često restriktivnijom odlukom.
- Konfiguracija ne menja smisao politike bez vidljivog procesa odobravanja.
- Bezbednosna kontrola ima vlasnika i očekivani dokaz, inače niko ne prati da li je prestala da radi.

### 3.5. Vidljivost kao sposobnost tima

Opservabilnost podrazumeva da se za značajnu odluku može pronaći odgovarajući kontekst: mesto nastanka, tok kroz slojeve sistema i razlog uspešnog ili odbijenog ishoda. Korelacioni identifikator povezuje zapise koji pripadaju istoj operaciji.

- Tehnički logovi razlikuju se od bezbednosno značajnih revizijskih zapisa.
- Metrika ima naziv, vlasnika i precizno definisano pitanje na koje odgovara.
- Tajne, tokeni i nepotrebni lični podaci ne beleže se u logove niti u oznake metrika.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 3.6. Šema od pravila do dokaza

### Šema od pravila do dokaza

- Politika: opisana namera i opseg
- Konfiguracija: važeće vrednosti i pragovi
- Kontrola: sprovodi odluku
- Dokaz: pokazuje stvarni ishod

Pravilo može biti dobro napisano, a kontrola ipak neefektivna. Dokaz povezuje očekivanje sa realnim ponašanjem.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 3.7. Politika, procedura i kontrola

| Element | Uloga | Primer dokaza |
| --- | --- | --- |
| Politika | Određuje pravilo i cilj zaštite. | Verzionisan tekst sa vlasnikom. |
| Procedura | Opisuje kako ljudi ili sistem sprovode pravilo. | Pregled koraka ili automatizovani tok. |
| Kontrola | Sprečava, otkriva ili ograničava rizik. | Test, log ili rezultat provere. |
| Metrika | Pokazuje trend ili odstupanje. | Definisan prag i izvor podataka. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 3.8. Kako se bira bezbednosna metrika

Metrika je korisna samo ako vodi ka odluci. Zato se ne počinje brojem, već pitanjem koje operater ili vlasnik sistema mora da reši. Zatim se bira izvor, period posmatranja i prag pri kome je potrebna akcija.

1. Koje ponašanje želimo da razumemo ili ograničimo?
2. Koji događaji pouzdano predstavljaju to ponašanje?
3. Ko je vlasnik praga i šta radi kada se prag pređe?
4. Kako razlikujemo normalnu promenu rada od bezbednosnog odstupanja?

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Politike, konfiguracija i bezbednosna vidljivost". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 3.9. Izvršivi primer

`ConfigurationDriftDetector` poredi aktivnu konfiguraciju sa usvojenim baseline-om. Kontrole implementiraju `IConfigurationControl`; nalaz označava slabljenje zaštite.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-03-politike-konfiguracija
dotnet run --project src/Oib.Vezba03.ConsoleUi
dotnet test Oib.Vezba03.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Minimalna dužina lozinke ili zahtev za administratorski MFA oslabi u odnosu na baseline. |
| Testovi | `ConfigurationControlTests`, `DetectConfigurationDriftHandlerTests` |
| Dokaz | `DriftReport` sadrži correlation ID i verziju baseline-a. |

> **Zadatak uz primer**
>
> Dodati kontrolu za maksimalno trajanje sesije: proširiti `SecurityBaseline` i `SecurityConfiguration`, napisati novu `IConfigurationControl` klasu i test koji pokazuje da duža sesija od dozvoljene daje nalaz.
