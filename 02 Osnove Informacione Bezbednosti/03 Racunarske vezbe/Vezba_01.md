# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

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
