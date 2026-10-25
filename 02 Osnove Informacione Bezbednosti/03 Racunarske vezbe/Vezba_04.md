# Osnove informacione bezbednosti — Praktikum

FTN · Nastavni materijali

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
