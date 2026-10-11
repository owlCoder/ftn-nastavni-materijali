# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

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
