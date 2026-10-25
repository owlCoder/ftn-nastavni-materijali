# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

## 0.1. Kako koristiti praktikum

Praktikum prati sadržaj vežbi iz informacione bezbednosti. Svaka tema obuhvata problem, primenjene kontrole, tipične greške i način provere donete odluke. Nakon vežbe student treba da ume da obrazloži obrađeni princip i primeni ga u okviru dodeljene projektne celine.

> **Primena na projektu**
>
> Svaki tim radi na svim dodeljenim projektnim celinama iz SCUTUM specifikacije i obrađene principe primenjuje na sopstveni domen. Dve kontrolne tačke, KT1 u terminu 4. vežbe i KT2 u terminu 8. vežbe, prate napredak celog dodeljenog obuhvata; dodatni zahtevi za timove sa tri celine navedeni su posebno.

| Faza | Preporučeni način rada |
| --- | --- |
| Pre vežbe | Pročitati temu i izdvojiti pretpostavke koje bi u informacionom sistemu mogle biti pogrešne. |
| Tokom vežbe | Povezati pojam sa posledicama u razvoju informacionog sistema, a ne samo sa nazivom klase ili krajnje tačke API-ja. |
| Posle vežbe | Primeniti princip na primeru sistema i dokumentovati rezultat testom, odlukom ili revizijskim zapisom. |
| Pred odbranu | Objasniti problem, izabranu kontrolu, njeno ograničenje i način na koji je ponašanje provereno. |

> **Defanzivna orijentacija**
>
> Praktikum ne zahteva razvoj eksploita, malvera ni napad na realne sisteme. Sumnjiva aktivnost, greška konfiguracije ili pokušaj nedozvoljenog pristupa reprodukuju se kontrolisanim simulatorima i testnim identitetima.

> **Jezik i alati**
>
> Primeri su pretežno u C#/.NET okruženju. Druga tehnologija može biti odobrena kada tim obezbedi interoperabilnost i ekvivalentan nivo testiranja i bezbednosne kontrole.

## 0.2. Tok gradiva

Osam vežbi obrađuje identitet i pristup, zaštitu podataka, bezbednosne politike, granice poverenja, reagovanje na incidente, procenu rizika i unapređivanje kontrola. Teme su raspoređene tako da se svaka naredna oslanja na prethodno uvedene pojmove.

- Identitet i RBAC: ko pristupa sistemu, sa kojim pravom i zašto.
- Autorizacija i podaci: odluka nad konkretnim resursom i osetljivost informacije.
- Politike i granice poverenja: pravila, konfiguracija, imovina i modelovanje pretnji.
- Operativna bezbednost: sesije, MFA, tajne, detekcija i incident.
- Napredne odluke: kontekstualna pravila, rizik, korelacija i dokaz efektivnosti.

Kontrolne tačke se održavaju u terminima 4. i 8. vežbe: KT1 proverava osnovnu implementaciju i bezbednosni model, a KT2 završnu demonstraciju i security QA. Kontrolne tačke i termini odbrane projekta su odvojene obaveze. Teme vežbi daju podršku radu, dok redosled implementacije svih dodeljenih celina tim određuje prema specifikaciji i njihovim preduslovima.

- KT1 (Vežba 1–4, Vežba 4) — Osnovna implementacija i bezbednosni model.
- KT2 (Vežba 5–8, Vežba 8) — Završna demonstracija i security QA.

## Vežba 1 — Identitet, autentikacija i RBAC

Bezbednost počinje razdvajanjem identiteta, prijave i ovlašćenja. Sistem najpre treba da pouzdano prepozna subjekat, zatim da proceni njegov aktuelni status, a tek potom da proveri šta taj subjekat sme da uradi. Kada se ti koraci pomešaju, greške se lako sakriju iza jedne uspešne prijave.

RBAC je važan zato što prava ne dodeljuje proizvoljno svakom nalogu. Uloge izražavaju odgovornosti u organizaciji, pa tim može razumljivo da upravlja pristupom, da ga pregleda i da ga ukine kada više nije potreban.

### Ko pristupa sistemu i zašto identitet prethodi svakoj odluci

- Identitet: subjekat sa životnim ciklusom
- Autentikacija: potvrda ko je subjekat
- Uloga: poslovna odgovornost
- Dozvola: konkretna dopuštena radnja

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 1.1. Mentalni model pristupa

RBAC je važan zato što prava ne dodeljuje proizvoljno svakom nalogu. Uloge izražavaju odgovornosti u organizaciji, pa tim može razumljivo da upravlja pristupom, da ga pregleda i da ga ukine kada više nije potreban.

- Uspešna prijava potvrđuje identitet, ali sama po sebi ne daje pravo na resurs.
- Uloga treba da bude čitljiva poslovna odgovornost, a dozvola mala i proverljiva radnja.
- Promena statusa ili uloge menja pristup odmah, bez oslanjanja na zastarelu pretpostavku klijenta.

> **Pitanje za diskusiju**
>
> Za jednu administrativnu radnju objasnite razliku između identiteta, uloge, dozvole i opsega u kome dozvola važi.

### 1.2. Kako se ova tema vidi u razvoju

Dizajn počinje pitanjima ko su akteri, koje odgovornosti imaju i nad kojim opsegom rade. Server donosi konačnu odluku; interfejs samo pomaže korisniku da ne bira radnje koje verovatno neće proći.

#### Česte greške koje treba prepoznati

- Mešanje autentikacije i autorizacije u jednu proveru.
- Davanje široke uloge zato što je brže od oblikovanja jedne dozvole.
- Oslanjanje na sakriveno dugme kao na bezbednosnu kontrolu.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 1.3. Primer: operator dobija novu odgovornost

Operater je do juče pregledao izveštaje, a danas privremeno pomaže timu koji odobrava promene na osetljivim resursima. Najjednostavnije rešenje deluje kao dodela administratorske uloge. Međutim, ta odluka često otvara mnogo više radnji nego što je stvarno potrebno, a kasnije je teško utvrditi koje pravo je bilo opravdano i kada treba da nestane.

- Prvo se određuje konkretna radnja koju operator treba da izvrši, a ne naziv najšire postojeće uloge.
- Zatim se proverava opseg: da li pravo važi za ceo sistem, jedan domen ili samo ograničeno vreme.
- Na kraju se definiše revizija ili istek, kako privremena potreba ne bi ostala trajna privilegija.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 1.4. Dizajnerska odluka: uloga nije dozvola

Uloga je način da organizacija grupiše odgovornosti, dok dozvola izražava najmanju radnju koju sistem štiti. To razdvajanje deluje kao dodatni posao na početku, ali smanjuje cenu svake kasnije promene. Kada se doda nova radnja, tim ne mora da izmišlja novu kombinaciju korisnika i izuzetaka, već procenjuje kojoj odgovornosti pripada i nad kojim opsegom važi.

- Uloge treba imenovati po odgovornosti, ne po tehničkom sloju aplikacije.
- Dozvola treba da opisuje radnju i resurs dovoljno precizno da može da se proveri.
- Efektivna prava nastaju iz uloge, opsega i aktuelnog statusa, a ne iz jednog statičkog polja korisnika.

### 1.5. Kako znamo da model pristupa radi

Najkorisniji dokaz nije samo uspešna prijava. Potrebno je pokazati i da sistem odbija radnju kada izostane jedna potrebna činjenica: aktivan identitet, odgovarajuća dozvola ili ispravan opseg. Takvi negativni slučajevi štite model od slučajnog širenja prava kroz kasnije izmene.

- Testovi porede očekivano dopuštenu i odbijenu radnju nad istim tipom resursa.
- Revizijski zapis beleži ko je pokušao radnju, šta je zahtevano i kakav je ishod.
- Pregled prava treba da objasni osnov pristupa, a ne samo naziv korisničke uloge.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 1.6. Šema odluke o pristupu

### Šema odluke o pristupu

- Zahtev: ko i šta traži
- Potvrda identiteta: da li je subjekat aktivan
- Provera dozvole: uloga i opseg
- Revizijski ishod: dopušteno ili odbijeno

Prijava je samo jedna faza. Pravo pristupa nastaje tek kada sistem proveri ceo kontekst.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 1.7. Poređenje osnovnih pojmova identiteta

| Pojam | Pitanje na koje odgovara | Tipična greška |
| --- | --- | --- |
| Identitet | Ko je subjekat? | Mešanje osobe i tehničkog naloga. |
| Autentikacija | Da li je tvrdnja o identitetu potvrđena? | Uspešna prijava se tumači kao široko ovlašćenje. |
| Autorizacija | Da li je radnja dopuštena? | Provera se radi samo u korisničkom interfejsu. |
| Sesija | U kom privremenom kontekstu pristup važi? | Token se tretira kao trajno pravo. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 1.8. Tok razmišljanja pri odluci o pristupu

Kada sistem primi zahtev, korisno je posmatrati odluku kao niz pitanja, a ne kao jednu metodu koja vraća da ili ne. Svako pitanje smanjuje skup mogućih grešaka i ostavlja jasan trag za kasniji pregled.

1. Da li je subjekat poznat i aktivan?
2. Da li je zahtevana radnja definisana kao dozvola?
3. Da li uloga i opseg daju to pravo baš u ovom trenutku?
4. Koji revizijski zapis opisuje uspeh ili odbijanje?

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Identitet, autentikacija i RBAC". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 1.9. Izvršivi primer

Autorizacija pristupa izveštajima na osnovu uloga. `RbacPolicy` podrazumevano odbija pristup; prijavljen nalog mora imati traženu dozvolu.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-01-identitet-rbac
dotnet run --project src/Oib.Vezba01.ConsoleUi
dotnet test Oib.Vezba01.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Uloga bez dozvole `reports:export` pokušava izvoz. |
| Testovi | `RbacPolicyTests`, `AuthorizeAccessHandlerTests` |
| Dokaz | `AccessAuditEntry` beleži i odobren i odbijen pristup. |

> **Zadatak uz primer**
>
> Dodati atribut `IsActive` akteru i pravilo da deaktiviran nalog nema pristup iako ima ulogu. Pravilo dodati u `RbacPolicy`, uvesti nov kod odluke i pokriti ga testom.

## Vežba 2 — Autorizacija nad resursom i klasifikacija podataka

Autorizacija ima smisla tek kada je vezana za konkretan resurs i njegov kontekst. Korisnik može imati legitimnu ulogu, a ipak ne sme da čita, menja ili deli baš taj zapis. Zato se odluka ne završava proverom tipa endpointa ili naziva uloge.

Klasifikacija podataka daje isti jezik vlasnicima, razvoju i operacijama. Kada sistem zna koliko je podatak osetljiv, može da primeni odgovarajući pristup, prikaz, čuvanje i evidenciju bez oslanjanja na nečije sećanje.

### Zašto poznavanje adrese resursa ne znači pravo pristupa

- Subjekat: ko traži radnju
- Resurs: nad čim se radi
- Kontekst: vlasništvo i namena
- Odluka: dozvoli ili odbij

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 2.1. Odluka mora imati kontekst

Klasifikacija podataka daje isti jezik vlasnicima, razvoju i operacijama. Kada sistem zna koliko je podatak osetljiv, može da primeni odgovarajući pristup, prikaz, čuvanje i evidenciju bez oslanjanja na nečije sećanje.

- Provera prava uključuje subjekt, radnju, resurs i njegovu pripadnost.
- Odbijanje ne treba da otkriva sadržaj niti postojanje tuđeg osetljivog resursa.
- Klasifikacija nije etiketa za dokumentaciju, već ulaz u pravila rukovanja podacima.

> **Pitanje za diskusiju**
>
> Koji resurs nosi rizik horizontalnog pristupa i koje informacije odluka mora da uzme u obzir?

### 2.2. Kako se ova tema vidi u razvoju

Za svaku značajnu operaciju tim formuliše i pozitivan i negativan scenario. Negativan scenario proverava da pravilno prijavljen korisnik ne može da zaobiđe vlasništvo promenom identifikatora ili pozivom API-ja van interfejsa.

#### Česte greške koje treba prepoznati

- Provera uloge bez provere odnosa prema konkretnom resursu.
- Filtriranje liste u klijentu bez iste provere na serveru.
- Snižavanje klasifikacije bez obrazloženja i traga promene.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 2.3. Primer: ispravna uloga, pogrešan zapis

Dva člana tima imaju istu ulogu i obojica mogu da pregledaju zahteve. Jedan menja identifikator u adresi zahteva i dolazi do tuđeg zapisa. Sistem koji proverava samo ulogu smatra zahtev dozvoljenim, iako se promenio najvažniji deo konteksta: odnos korisnika prema konkretnom resursu.

- Odluka uključuje identitet, traženu radnju, resurs i pripadnost resursa odgovarajućem domenu ili vlasniku.
- Isti princip važi za API poziv, izvoz podataka i pozadinsku obradu, ne samo za ekran koji korisnik vidi.
- Odgovor na odbijeni pristup treba da bude koristan klijentu, ali ne sme da otkrije podatke o zaštićenom zapisu.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 2.4. Dizajnerska odluka: podatak nosi obaveze

Klasifikacija ne govori samo koliko je podatak važan. Ona utiče na način prikaza, izvoz, deljenje, čuvanje i brisanje. Kada te posledice nisu jasno vezane za klasifikaciju, svaki novi ekran ili integracija mora ponovo da pogađa kako treba rukovati informacijom.

- Klasifikacija ima poslovno značenje koje svi članovi tima tumače na isti način.
- Pravila za prikaz i izvoz treba da budu stroža kada posledice greške rastu.
- Vlasnik podatka odgovara za opravdanost promene klasifikacije, a sistem čuva trag te promene.

### 2.5. Kako znamo da je autorizacija potpuna

Pristup treba proveriti kroz stvarnu granicu sistema, a ne samo kroz korisnički interfejs. To znači da direktan API zahtev, pozadinski proces i izvoz ne mogu preskočiti isto pravilo. Negativni testovi zato nisu dodatak posle implementacije, već najjasniji opis granice pristupa.

- Sistem jednako štiti resurs kada se zahtev pošalje van standardnog ekrana.
- Odbijeni zahtev ne vraća detalje koji pomažu da se mapiraju tuđi podaci.
- Promena klasifikacije i promena vlasništva ostavljaju dovoljno traga za kasniji pregled.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 2.6. Šema zaštite jednog resursa

### Šema zaštite jednog resursa

- Resurs: vlasnik i klasifikacija
- Radnja: čitanje, izmena ili izvoz
- Serverska provera: subjekat, odnos i pravilo
- Bezbedan ishod: podaci ili kontrolisano odbijanje

Identifikator u adresi ne predstavlja pravo pristupa. Odnos subjekta prema resursu proverava se na serveru.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 2.7. Nivoi provere pristupa

| Nivo | Šta proverava | Zašto nije dovoljan sam |
| --- | --- | --- |
| Prijava | Identitet subjekta. | Ne govori šta subjekat sme da uradi. |
| Uloga | Opšti skup odgovornosti. | Ne zna odnos prema konkretnom zapisu. |
| Resurs | Vlasništvo, pripadnost i klasifikaciju. | Mora se kombinovati sa radnjom i politikom. |
| Kontekst | Vreme, stanje i poslovno opravdanje. | Može promeniti zaključak za istu ulogu. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 2.8. Životni ciklus podatka

Zaštita podataka ne završava se u trenutku čitanja. Isti podatak prolazi kroz kreiranje, upotrebu, deljenje, čuvanje i uklanjanje. Svaka faza postavlja drugo pitanje o dozvoljenom pristupu i potrebnom tragu.

1. Pri kreiranju se određuju vlasnik i klasifikacija.
2. Pri korišćenju se proverava svrha i opseg pristupa.
3. Pri deljenju se proverava granica poverenja i minimalni skup podataka.
4. Pri čuvanju i uklanjanju primenjuju se rokovi čuvanja i pravila revizijskog evidentiranja.

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Autorizacija nad resursom i klasifikacija podataka". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 2.9. Izvršivi primer

`ResourceReadPolicy` proverava dozvolu, vlasništvo i klasifikaciju konkretnog zapisa. Provera se izvršava na serveru za svaki zahtev.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-02-resurs-klasifikacija
dotnet run --project src/Oib.Vezba02.ConsoleUi
dotnet test Oib.Vezba02.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Korisnik pogađa identifikator tuđeg zapisa. |
| Testovi | `ResourceReadPolicyTests`, `ReadResourceHandlerTests` |
| Dokaz | `ResourceAccessAuditEntry` sadrži ishod i stabilan kod odluke. |

> **Zadatak uz primer**
>
> Uvesti radnju izmene zapisa: menja ga samo vlasnik, bez obzira na `records:read:any`. Dodati novu politiku i use-case pored postojećih, bez menjanja `ResourceReadPolicy`.

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

## Vežba 4 — Imovina, granice poverenja i modelovanje pretnji

Dobra bezbednosna odluka ne polazi od liste poznatih rešenja, već od vredne imovine, toka podataka i granica poverenja. Tek tada tim može da prepozna šta bi moglo poći po zlu i koja kontrola ima smisla za konkretan rizik.

Modelovanje pretnji povezuje tehničku strukturu sistema sa poslovnim posledicama i omogućava da se pogrešne pretpostavke uoče pre nego što postanu deo stabilne arhitekture.

### Razumevanje onoga što štitimo pre izbora kontrole

- Imovina: vredan podatak ili servis
- Tok podataka: kretanje informacije
- Granica poverenja: promena pretpostavki
- Pretnja i kontrola: rizik i odgovor

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 4.1. Model pre kontrole

Modelovanje pretnji povezuje tehničku strukturu sistema sa poslovnim posledicama i omogućava da se pogrešne pretpostavke uoče pre nego što postanu deo stabilne arhitekture.

- Imovina ima vlasnika, značaj i životni ciklus.
- Spoljna komponenta ne dobija poverenje samo zato što je integrisana.
- Pretnja postaje korisna kada vodi do zahteva, kontrole i načina provere.

> **Pitanje za diskusiju**
>
> Nacrtajte jedan tok osetljivog podatka i označite mesto na kome se pretpostavka o poverenju menja.

### 4.2. Kako se ova tema vidi u razvoju

Arhitekturni dijagram postaje osnova bezbednosne analize kada prikazuje podatke, aktere i pretpostavke uz svaku važnu granicu. Tim zatim bira nekoliko realnih scenarija zloupotrebe i proverava ih testom ili pregledom dizajna.

#### Česte greške koje treba prepoznati

- Inventar koji postoji samo radi evidencije, bez vlasništva i značaja.
- Opšti katalog pretnji koji nije povezan sa stvarnim sistemom.
- Kontrola izabrana pre razumevanja rizika koji treba da smanji.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 4.3. Primer: izvoz iz internog sistema

Interni servis priprema izvoz osetljivih podataka za drugi sistem. Tim ga doživljava kao bezbedan jer je servis „unutra“, ali sam prelazak podataka u drugi sistem menja pretpostavke. Potrebno je znati koji podatak putuje, ko prima podatak, šta može poći po zlu i kako sistem sprečava ili bar otkriva neželjen ishod.

- Imovina obuhvata naloge, ključeve, servisne identitete, konfiguraciju i poslovne zapise, a ne samo bazu podataka.
- Granica poverenja može biti API poziv, red poruka, administrativni interfejs ili izvoz u datoteku.
- Model pretnji obuhvata odabrane realne scenarije, odgovarajuće kontrole i način njihove provere.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 4.4. Dizajnerska odluka: prvo model, zatim alat

Izbor bezbednosnog alata zasniva se na imovini koju treba zaštititi, granici koju treba nadzirati i scenariju koji treba otkriti. Model pretnji usmerava izbor kontrole i omogućava procenu njene vrednosti.

- Kritičnost imovine određuje koliko brzo se reaguje i koliko je dokaza potrebno.
- Tok podataka otkriva mesta na kojima informacija može biti izmenjena, izgubljena ili pogrešno izložena.
- Pretnja dobija prioritet prema uticaju, verovatnoći i realnim postojećim kontrolama.

### 4.5. Održavanje modela pretnji

Model pretnji nije dokument koji se napiše jednom na početku semestra. Menja se kada se doda novi ulaz, integracija, način čuvanja podataka ili privilegovana radnja. Svaka veća arhitektonska promena je prilika da se proveri da li su stare pretpostavke i dalje tačne.

- Dijagram se ažurira kada tok podataka ili granica poverenja stvarno promeni ponašanje sistema.
- Za odabrani scenario postoji veza do kontrole, testa ili pregleda koji ga proverava.
- Preostali rizik se zapisuje jasno, umesto da se pretvara da nijedna pretnja više ne postoji.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 4.6. Šema modela pretnji

### Šema modela pretnji

- Imovina: šta štitimo
- Tok i granica: gde se menja poverenje
- Scenario: šta može poći po zlu
- Kontrola i dokaz: kako umanjujemo rizik

Pretnja dobija smisao tek kada se veže za konkretnu imovinu, tok i posledicu.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 4.7. Imovina, ranjivost i pretnja

| Pojam | Značenje | Pitanje za analizu |
| --- | --- | --- |
| Imovina | Ono čija vrednost zahteva zaštitu. | Šta gubimo ako je ugrožena? |
| Ranjivost | Slabost koju je moguće iskoristiti. | Gde sistem nema dovoljnu zaštitu? |
| Pretnja | Mogući uzrok neželjenog ishoda. | Ko ili šta može aktivirati slabost? |
| Rizik | Kombinacija verovatnoće i posledice. | Šta prvo treba umanjiti? |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 4.8. Čitanje arhitektonske skice kroz bezbednost

Arhitektonska skica dobija bezbednosnu vrednost kada se preko komponenti ucrtaju podaci, identiteti i granice poverenja. Nije potrebno crtati svaku klasu. Dovoljno je pokazati delove kroz koje prolazi vredna informacija i odluke koje menjaju njen status.

1. Označiti ulaze koje kontroliše korisnik ili spoljašnji servis.
2. Označiti mesto gde se identitet ili dozvola ponovo proveravaju.
3. Označiti skladišta, ključeve i integracije koje menjaju posledicu incidenta.
4. Uz svaku granicu zapisati jednu realnu pretpostavku koja može biti pogrešna.

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „Imovina, granice poverenja i modelovanje pretnji". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 4.9. Izvršivi primer

Procena scenarija pretnje povezuje imovinu, tok podataka i granicu poverenja. `ThreatRiskCalculator` računa rizik; demonstraciona politika zahteva tretman pri vrednosti 24 ili većoj.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-04-threat-modeling
dotnet run --project src/Oib.Vezba04.ConsoleUi
dotnet test Oib.Vezba04.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Izvoz ličnih podataka stiže neovlašćenom primaocu. |
| Testovi | `ThreatRiskCalculatorTests`, `AssessThreatModelHandlerTests` |
| Dokaz | `ThreatModelReport` prikazuje scenarije po riziku i predložene kontrole. |

> **Zadatak uz primer**
>
> Dodati rezidualni rizik: `ThreatScenario` dobija procenu efikasnosti kontrole, a izveštaj prikazuje rizik pre i posle kontrole. Pravilo smestiti u domenski servis i pokriti graničnim testovima.

### Kontrolna tačka KT1 — Osnovna implementacija i bezbednosni model

Prva kontrolna tačka održava se u terminu 4. vežbe. Tim prikazuje obuhvat svih dodeljenih celina, bezbednosni model i prvi zaokružen funkcionalni tok sa serverskom zaštitom i negativnim testom.

#### Obavezno za sve timove

1. Za SVAKU dodeljenu celinu: lista use-case-ova iz specifikacije, redosled implementacije i označeni preduslovi (TAPIZ backlog).
2. Jedan objedinjeni domen model / skica toka, ključni entiteti, statusi i vlasništvo podataka; minimalna security matrica Subject–Action–Resource–Allow/Deny za relevantne operacije.
3. Repozitorijum: osnovna struktura aplikacije, README sa startovanjem, build koji prolazi, početni CI i Tapiz task → branch → PR praksa.
4. Primarna celina: najmanje jedan zaokružen funkcionalan tok koji prolazi kroz aplikacionu logiku i persistence/simulator, sa demonstracijom normalnog ishoda; za jednostavnije celine očekuju se bar dve ključne operacije (npr. unos + promena statusa).
5. Serverska zaštita konkretne operacije (scope/ownership/role prema domenu), validacija ulaza i jedan AUTOMATIZOVAN negativni test; ne smatra se dovoljnom zaštita samo kroz UI.
6. Threat model svih dodeljenih celina: asset → misuse/pretnja → kontrola → test → evidence, uz prikaz bar jedne trust granice.
7. Migracije/seed ili ekvivalentno ponovljivo testno stanje; strukturisani događaj/audit za relevantnu promenu bez tajni.
8. Minimalni cross-team ugovor pregledan sa susednim timom; ako implementacija nije spremna, stub + pozitivni i negativni odgovor.

> **Prikaz**
>
> Model i backlog, pokrenut normalan scenario, negativan automatski test i konkretan plan završetka do KT2 u terminu 8. vežbe.

> **Ne tražiti u ovoj fazi**
>
> Puna realizacija druge/treće celine. Produkcioni simulator.

> **Termin i obuhvat**
>
> Vežba 1–4 · Vežba 4 Isti zahtevi prikazani su na kartici „Kontrolne tačke“.

## Vežba 5 — MFA, sesije, servisi i tajne

Identitet nije trajno dokazan jednom prijavom. Rizik radnje, trajanje sesije, vrsta uređaja i promena privilegija utiču na to koliko je dodatna potvrda potrebna. Operativna bezbednost zato prati pristup i nakon početne autentikacije.

Višefaktorska potvrda i vremenski ograničene privilegije smanjuju posledice ukradenog kredencijala. Upravljanje tajnama štiti veze među servisima, gde nepažnja često ostane skrivena jer nema korisničkog ekrana.

### Kontrola pristupa kroz vreme, a ne samo u trenutku prijave

- Prijava: početna potvrda identiteta
- Sesija: privremeni kontekst pristupa
- Dodatna potvrda: za rizičnu radnju
- Istek ili opoziv: prekid važenja pristupa

Bezbednosna odluka nastaje povezivanjem prikazanih pojmova.

> **Cilj vežbe**
>
> Student treba da obrazloži odluku sistema, podatke na kojima se ona zasniva i njen uticaj na zaštitu podataka i usluga.

### 5.1. Pristup ima rok i kontekst

Višefaktorska potvrda i vremenski ograničene privilegije smanjuju posledice ukradenog kredencijala. Upravljanje tajnama štiti veze među servisima, gde nepažnja često ostane skrivena jer nema korisničkog ekrana.

- Sesija može izgubiti važnost pre planiranog isteka.
- Dodatna potvrda prati rizik radnje, ne samo tip korisnika.
- Servisni identitet ima mali, jasno opravdan opseg, kao i ljudski identitet.

> **Pitanje za diskusiju**
>
> Koja radnja zaslužuje dodatnu potvrdu identiteta i koji događaj bi trebalo da opozove postojeću sesiju?

### 5.2. Kako se ova tema vidi u razvoju

Razvojni tim unapred određuje koje situacije menjaju nivo poverenja: promena privilegije, pristup osetljivom podatku ili sumnjiva prijava. Tajne se referenciraju iz bezbednog izvora i nikada se ne prenose kroz kod, testne ispise ili istoriju repozitorijuma.

#### Česte greške koje treba prepoznati

- Token tretiran kao večna dozvola.
- Administratorska prava dodeljena bez roka i pregleda.
- Jedna široka tajna deljena među više nezavisnih servisa.

> **Veza sa ostalim temama**
>
> Razmatrana odluka povezuje identitet, podatke, politike i dokaze, a isti postupak analize primenjuje se na različite vrste informacionih sistema.

### 5.3. Primer: sesija posle promene privilegije

Korisnik se prijavio ujutru, a kasnije tokom dana dobija viši nivo pristupa. Ako sistem ne preispita postojeću sesiju, korisnik može izvršiti rizičnu radnju bez dodatne potvrde koja bi bila potrebna da se prijavljuje u tom trenutku. Obrnuto, oduzeta privilegija mora zaista da prestane da važi, a ne da čeka kraj dugotrajne sesije.

- Sesija predstavlja privremeni kontekst pristupa, a ne trajnu potvrdu poverenja.
- Dodatna autentikacija primenjuje se kada rizik radnje zahteva viši nivo potvrde identiteta.
- Servisni nalog ima najmanji potreban opseg, rok važenja i kontrolisanu rotaciju tajne.

> **Kriterijumi rešenja**
>
> Rešenje treba da bude obrazloženo poslovnim kontekstom, primenjeno na serverskoj strani i potvrđeno ponovljivim testom, revizijskim zapisom ili demonstracijom.

### 5.4. Dizajnerska odluka: poverenje se obnavlja

Sesija olakšava rad korisniku, ali njen komfor ne sme postati trajno poverenje. Sistem mora znati kada se okolnosti menjaju dovoljno da prethodna potvrda više nije dovoljna. To mogu biti promena privilegije, neobičan uređaj, rizična radnja ili signal da je nalog ugrožen.

- Istek ograničava trajanje poverenja čak i kada nema vidljivog incidenta.
- Opoziv reaguje na konkretnu promenu rizika i ne čeka da sesija prirodno nestane.
- Dodatni faktor dokazuje prisustvo ili posedovanje nezavisnog sredstva, a ne ponavlja samo istu lozinku.

### 5.5. Tajne kao deo arhitekture

Tajna nije konfiguraciona vrednost kao svaka druga. Njeno izlaganje može dati pristup bez vidljivog korisničkog ekrana, naročito između servisa. Zato razvojni proces definiše odakle tajna dolazi, ko je koristi, kada se rotira i kako se proverava da nije slučajno završila u repozitorijumu ili logu.

- Svaki servisni identitet ima svrhu i najmanji potreban opseg.
- Rotacija se planira pre isteka, umesto da se čeka prekid rada.
- Testovi proveravaju ponašanje pri opozivu ili isteku bez prikazivanja stvarnih vrednosti tajne.

> **Samostalna provera**
>
> Za izabrani scenario navedite odluku sistema, korišćene podatke, očekivani ishod i dokaz na osnovu kog drugi član tima može da potvrdi rezultat.

### 5.6. Šema životnog ciklusa poverenja

### Šema životnog ciklusa poverenja

- Početna prijava: osnovni nivo poverenja
- Aktivna sesija: vremenski ograničena
- Rizična radnja: dodatna potvrda ili odbijanje
- Opoziv ili istek: pristup prestaje da važi

Poverenje se ne čuva zauvek. Sistem ga obnavlja ili opoziva kada se promeni rizik.

Za svaki korak šeme odredite pokretača, odluku koja se donosi i zapis koji ostaje nakon izvršenja.

### 5.7. Različite vrste identiteta

| Identitet | Namena | Poseban rizik |
| --- | --- | --- |
| Ljudski korisnik | Interaktivne radnje i odgovornost. | Krađa kredencijala ili preširoka uloga. |
| Servisni nalog | Komunikacija među komponentama. | Tajna bez roka ili prevelik opseg. |
| Privremeni pristup | Kratka rizična intervencija. | Pravo ostane aktivno nakon potrebe. |
| Administratorski nalog | Izmene sa velikim uticajem. | Nedostatak dodatne potvrde i audita. |

> **Tumačenje poređenja**
>
> Svaki pojam opisuje poseban deo bezbednosne odluke; tek njihovom pravilnom kombinacijom nastaje celovit model.

### 5.8. Od tajne do bezbedne upotrebe

Bezbedno upravljanje tajnom počinje pre njenog prvog korišćenja. Potrebno je znati vlasnika, svrhu, komponentu koja je koristi i način rotacije. Sama vrednost tajne treba da ostane van koda, ekrana i zapisa koji nisu za to namenjeni.

1. Tajna se izdaje samo identitetu koji joj je stvarno potreban.
2. Sistem prati istek i planira rotaciju pre prekida rada.
3. Logovi beleže upotrebu i neuspeh bez prikazivanja vrednosti tajne.
4. Opoziv ograničava posledicu sumnje da je tajna kompromitovana.

> **Ishod učenja**
>
> Student treba da ume da izabere odgovarajuća pitanja za analizu bezbednosno značajne odluke i da obrazloži redosled njihove primene.

> **Primena na projektu**
>
> Na dodeljenoj projektnoj celini primeniti princip iz teme „MFA, sesije, servisi i tajne". Identifikovati odluku sistema, implementirati je na serverskoj strani i pripremiti dokaz u obliku testa, revizijskog zapisa ili demonstracije.

### 5.9. Izvršivi primer

`StepUpAuthenticationPolicy` proverava da li sesija ima dovoljno svežu MFA potvrdu za rizičnu operaciju. Odluka razlikuje odbijanje i potrebu za novom potvrdom.

Pokretanje iz repozitorijuma nastavnog portala; potreban je .NET 8 SDK.

```bash
cd examples/oib-dotnet/vezba-05-mfa-sesije-tajne
dotnet run --project src/Oib.Vezba05.ConsoleUi
dotnet test Oib.Vezba05.sln
```

| Provera | Sadržaj |
| --- | --- |
| Scenario | Preuzeta, opozvana ili davno potvrđena sesija pokušava poverljiv izvoz. |
| Testovi | `StepUpAuthenticationPolicyTests`, `AuthorizeRiskyOperationHandlerTests` |
| Dokaz | `StepUpDecision` vraća kod odluke i oznaku da li se traži MFA. |

> **Zadatak uz primer**
>
> Dodati maksimalnu starost same sesije: sesija izdata pre više od osam sati odbija se bez obzira na MFA. Uvesti nov kod odluke i test za graničnu vrednost.

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

### Kontrolna tačka KT2 — Završna demonstracija i security QA

Druga kontrolna tačka održava se u terminu 8. vežbe. Tim demonstrira sve obavezne use-case-ove svih dodeljenih celina u odobrenom obimu, njihovo povezivanje, bezbednosne odluke i ponovljivo pokretanje, uz ažurnu dokumentaciju.

#### Obavezno za sve timove

1. Svi obavezni use-case-ovi AKTIVNOG/ODOBRENOG scope-a celina; ako se puni normativni scope iz SCUTUM specifikacije smanjuje, NASTAVNI TIM mora to unapred eksplicitno objaviti kao reviziju.
2. End-to-end demo: početno stanje → akcija → odluka dozvoli/odbij → relevantan security/audit trag → konačan status.
3. Unit + negativni security + odgovarajući integracioni/contract testovi po relevantnoj celini, CI green, dokumentovane revizije PR.
4. Clean clone, migracije i seed/demo podaci rade prema README-u.
5. API/event/policy ugovori, threat model, ADR, evidencija testiranja, individualni doprinosi i residual risk su ažurni.
6. Cross-team integracija sa realnim adapterom gde je dostupna; samo stub ne prikazivati kao završenu realnu integraciju.

#### Dodatno za timove sa 3 celine

- Treća celina mora imati javno odobren opseg i izvršiv bezbednosni tok sa automatizovanim testovima; kompletan normativni obim je obavezan osim ako je nastavni tim formalno promenio specifikaciju.

> **Prikaz**
>
> Povezani end-to-end scenario, pozitivan i negativan test, ugovori i individualno obrazloženje rada. Kontrolne tačke i termini odbrane projekta su odvojene obaveze.

> **Termin i obuhvat**
>
> Vežba 5–8 · Vežba 8 Isti zahtevi prikazani su na kartici „Kontrolne tačke“.

## Sažetak: bezbednost kao sledljiv, dokaziv proces

Vežbe primenjuju isti analitički sled: imovina → pretnja ili zloupotreba → bezbednosni zahtev → kontrola → bezbednosni test → dokaz. Postupak se primenjuje na autentikaciju, privilegovani pristup i analizu putanja napada, uz prilagođavanje nivou posmatranog sistema.

| Nivo | Šta uvodi |
| --- | --- |
| R1 — osnovni | Identitet, resursi, klasifikacija, autorizacija, politike i audit. |
| R2 — operativni | MFA, sesije, tajne, detekcija, incidenti i ranjivosti. |
| R3 — napredni | Mehanizam politika, pregled pristupa, rizik, korelacija i automatizacija odgovora. |

> **Odgovornost studenta**
>
> Tim je odgovoran za razumevanje zahteva, obrazloženje bezbednosnog modela i dokazivanje efektivnosti primenjene kontrole.

## Preporučena literatura i dokumentacija

Literatura je namenjena produbljivanju tema iz praktikuma. Pojmove treba povezivati sa modelom pretnji, bezbednosnim testovima i revizijskim zapisima konkretnog informacionog sistema.

- OWASP — *Application Security Verification Standard (ASVS)* i *OWASP Top 10*: [owasp.org](https://owasp.org).
- NIST — *Digital Identity Guidelines (SP 800-63)*: [pages.nist.gov/800-63-3](https://pages.nist.gov/800-63-3/).
- NIST — *Zero Trust Architecture (SP 800-207)*: [csrc.nist.gov](https://csrc.nist.gov/publications/detail/sp/800-207/final).
- Adam Shostack — *Threat Modeling: Designing for Security*.
- NUnit dokumentacija: [docs.nunit.org](https://docs.nunit.org); Moq dokumentacija: [github.com/devlooped/moq](https://github.com/devlooped/moq).
- Microsoft Learn — ASP.NET Core Identity, autorizacija i bezbedna konfiguracija: [learn.microsoft.com/aspnet/core/security](https://learn.microsoft.com/aspnet/core/security/).
- MITRE ATT&CK — okvir za razumevanje taktika i tehnika (koristi se isključivo kao referentni rečnik, ne kao uputstvo za napad): [attack.mitre.org](https://attack.mitre.org).

> **Napomena o alatima**
>
> Konkretni bezbednosni alati (SIEM, IDS, PKI proizvodi) menjaju se brže od osnovnih principa. Simulatori korišćeni u praktikumu oponašaju njihovo ponašanje radi vežbe — ne predstavljaju se kao realni proizvodi.
