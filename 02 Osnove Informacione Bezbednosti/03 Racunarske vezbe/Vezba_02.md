# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

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
