# Dopuna kataloga projektnih celina OIB - 105 J i 105 P/N

**Datum:** 3. novembar 2026.  
**Osnovni dokument:** `Projektna_Specifikacija_OIB.docx` u direktorijumu `Osnove informacione bezbednosti (OIB)/Projekat`.  
**Status:** dopuna kataloga; postojeće oznake i kriterijumi se ne menjaju.

## Pravilo dodele i bilans

Svaki tim dobija **tačno jednu celinu J (jezgro)** i **tačno jednu celinu P (proširenje) ili N (napredno)**. Jedna oznaka se bez posebne odluke nastavnog tima dodeljuje najviše jednom timu. Par mora biti funkcionalno povezan, ali ne mora imati isti prefiks. Oznake iz kategorija SEC i OPS ostaju opšti zahtevi i ne računaju se kao samostalne celine.

| Kategorija | Postojeće | Nove | Ukupno |
|:--|--:|--:|--:|
| J | 59 | 46 | **105** |
| P | 33 | 37 | **70** |
| N | 20 | 15 | **35** |
| **P + N** | **53** | **52** | **105** |
| **Sve funkcionalne celine** | **112** | **98** | **210** |

**Kontrola balansa:** J = P + N = 105. Ova jednakost garantuje dovoljan broj jedinstvenih oznaka, ali ne i automatski kompatibilnost svake kombinacije. Neke N celine zavise od posebne infrastrukture, pa se dodela radi po zavisnostima i raspoloživim timovima.

## Opšta pravila za svaku novu celinu

1. Obavezni su serverska provera prava, validacija ulaza, čuvanje istorije gde je relevantno i najmanje jedan pozitivan i jedan negativan automatizovani test.
2. Celine koriste postojeće javne ugovore modula; ne pristupaju tuđim internim tabelama ili tajnama bez dogovorene integracije.
3. Za svaku celinu demonstrirati osnovni tok, negativni slučaj, audit događaj kada je značajan i uklopljenost u postojeći modularni monolit.
4. Kolona "Veze" navodi preduslove/integracione dodirne tačke, a ne dodatne obaveze da isti tim implementira sve navedene ID-jeve.

## Katalog 98 novih celina

### IDN - 6 novih (J 3, P 2, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| IDN-09 | P | Rezervni MFA kodovi | Svaki rezervni kod može da se iskoristi samo jednom; čuva se samo njegov bezbedan otisak i upotreba se evidentira. | IDN-06 |
| IDN-10 | J | Potvrda adrese elektronske pošte | Nalog kojem je potvrda obavezna ne može se aktivirati nevažećim, isteklim ili već upotrebljenim tokenom. | IDN-01 |
| IDN-11 | J | Rotacija identifikatora sesije | Posle prijave ili promene nivoa privilegija menja se identifikator sesije; prethodni identifikator se odbija. | IDN-02 |
| IDN-12 | J | Lična istorija bezbednosnih promena | Vlasnik naloga vidi vreme i vrstu promene statusa ili pristupnih podataka, ali ne i tajne ili podatke drugog naloga. | IDN-03, AUD-01 |
| IDN-13 | P | Upozorenje o novoj sesiji | Korisnik može uključiti obaveštenje za novu sesiju; poruka ne sadrži tokene niti osetljive identifikatore. | IDN-07, AUD-05 |
| IDN-14 | N | Rizikom uslovljena ponovna autentikacija | Osetljiva radnja zahteva novu potvrdu identiteta prema dokumentovanoj politici rizika; neuspešna potvrda ne odobrava radnju. | IDN-06, ACC-04 |

### ACC - 6 novih (J 3, P 2, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| ACC-09 | J | Prenos odgovornosti za kurs | Promenu vlasnika kursa može da potvrdi samo ovlašćena uloga; prenos ne ostavlja kurs bez odgovorne osobe. | ACC-01, ACC-03 |
| ACC-10 | J | Zaštita poslednjeg aktivnog nastavnika | Uklanjanje ili deaktivacija poslednjeg ovlašćenog nastavnika kursa se odbija uz objašnjenje i audit zapis. | ACC-03 |
| ACC-11 | J | Istorija članstava u kursu | Ispis studenta zatvara aktivno članstvo bez brisanja istorije; prethodni pokušaji ostaju vezani za ispravan kurs. | ACC-02, ATT-01 |
| ACC-12 | P | Zahtev za pridruživanje kursu | Student podnosi zahtev; upis nastaje tek nakon odobrenja ovlašćene osobe, a ponovljen zahtev ne kreira duplikat. | ACC-02 |
| ACC-13 | P | Paketni upis uz probni pregled | Uvoz liste prvo prijavljuje nevažeće i duplirane redove; potvrda obrađuje samo prihvaćene stavke. | ACC-02 |
| ACC-14 | N | Razdvajanje nespojivih dužnosti | Politika može zabraniti konfliktne kombinacije uloga pri promeni članstva; pokušaj zaobilaženja se odbija serverski. | ACC-03, ACC-04 |

### LAB - 8 novih (J 4, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| LAB-08 | P | Omiljene laboratorije i lične kolekcije | Korisnik održava svoju listu, ali iz nje ne može otvoriti laboratoriju za koju je izgubio pravo pristupa. | LAB-05, ACC-04 |
| LAB-09 | J | Javni sažetak laboratorije | Studentski prikaz sadrži samo dozvoljene metapodatke; skriveni evaluator, nacrti i ključevi nisu dostupni. | LAB-02, AUT-04 |
| LAB-10 | J | Jedinstvena čitljiva oznaka laboratorije | Unutar zadatog konteksta nije moguće dodeliti istu čitljivu oznaku dvema različitim laboratorijama; trajni ID se ne menja. | LAB-01 |
| LAB-11 | J | Uređivanje kontrolisanog rečnika oznaka | Duplirane oznake nakon normalizacije nisu dozvoljene; uklonjena oznaka ne briše istoriju ranijih laboratorija. | LAB-02 |
| LAB-12 | J | Evidencija odgovornog autora laboratorije | Promena autora zahteva dozvolu i beleži prethodnog i novog odgovornog korisnika; vlasništvo ne daje pristup tuđim kursevima. | LAB-03, ACC-04 |
| LAB-13 | P | Nastavničke tematske kolekcije | Nastavnik može sastaviti uređenu kolekciju dostupnih vežbi svog kursa; kolekcija ne zaobilazi stanje objave. | LAB-04, ACC-01 |
| LAB-14 | P | Kopiranje opisa između kurseva | Ovlašćeni nastavnik kopira dozvoljene metapodatke u novi nacrt sa novim ID-jem bez prenošenja skrivenih rezultata. | LAB-01, AUT-01 |
| LAB-15 | N | Vizuelizacija grafa preduslova | Prikaz povezanih laboratorija otkriva cikluse i nedostupne čvorove, bez prikaza privatnih vežbi drugim kursevima. | LAB-06, ASN-07 |

### AUT - 8 novih (J 4, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| AUT-09 | J | Provera konzistentnosti koraka nacrta | Nacrt nije spreman za objavu ako sadrži duplirane identifikatore koraka, nepostojeće reference ili pogrešan redosled. | AUT-02, AUT-03 |
| AUT-10 | J | Evidencija referenci na nastavničke priloge | Svaki prilog ima vlasnika, kontekst i status; uklonjen prilog ne može ostati aktivna referenca u novoj objavi. | AUT-05 |
| AUT-11 | J | Privatne beleške autora zadatka | Autor može čuvati interne beleške; studentski API i javni prikaz ih nikada ne iznose. | AUT-01, AUT-04 |
| AUT-12 | J | Kontrolisano automatsko čuvanje nacrta | Ponovljeni zahtev je idempotentan; zastarelo automatsko čuvanje ne prepisuje noviju verziju nacrta. | AUT-01 |
| AUT-13 | P | Promena redosleda koraka bez gubitka veza | Promena redosleda zadržava stabilne identifikatore i reference; pokvarene veze se prijavljuju pre potvrde. | AUT-02 |
| AUT-14 | P | Biblioteka deljivih blokova instrukcija | Autori ponovo koriste odobrene blokove sa fiksiranom verzijom; izmena izvornog bloka ne menja već objavljenu reviziju. | AUT-01, REV-01 |
| AUT-15 | P | Probna evaluacija u autorskom alatu | Autor izvršava probni odgovor u izolovanom nastavničkom toku, bez stvaranja studentskog pokušaja ili javnog rezultata. | AUT-04, EVAL-01 |
| AUT-16 | N | Migracija formata definicije scenarija | Stara šema nacrta migrira se uz izveštaj o promenama; neuspešna migracija ne menja originalne podatke. | AUT-03, REV-01 |

### REV - 7 novih (J 3, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| REV-08 | P | Zakazano objavljivanje i povlačenje revizija | Server sprovodi zakazanu promenu u zadatoj vremenskoj zoni; nevažeća ili neodobrena revizija ne postaje javna. | REV-02, REV-04 |
| REV-09 | J | Obavezno obrazloženje nove revizije | Objavljivanje bez evidentiranog opisa značajne izmene se odbija; opis ostaje povezan sa brojem revizije. | REV-01 |
| REV-10 | J | Kontrolni otisak objavljene definicije | Za svaku objavljenu reviziju čuva se kontrolni otisak; naknadna razlika između sadržaja i otiska se prijavljuje. | REV-01 |
| REV-11 | J | Povratak na raniju objavljenu definiciju | Povratak kopira prethodnu definiciju u novu reviziju; istorijski pokušaji zadržavaju originalnu vezu. | REV-01, REV-03 |
| REV-12 | P | Pregled kompatibilnosti novih revizija | Pre objave nastavnik dobija izveštaj o promenama polja i kriterijuma koji utiču na postojeće dodele. | REV-02, ASN-01 |
| REV-13 | P | Odvojeni javni i interni opis promena | Student vidi samo bezbedan opis promene; nastavnik sa dozvolom vidi i interne izmene evaluatora. | REV-06, AUT-04 |
| REV-14 | N | Analiza uticaja objave na aktivne pokušaje | Sistem utvrđuje pogođene dodele i pokušaje bez njihovog prevezivanja; kritičan konflikt blokira objavu do odluke. | REV-03, ASN-01, ATT-01 |

### ASN - 7 novih (J 3, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| ASN-08 | J | Zamrzavanje uslova dodele pri početku pokušaja | Započeti pokušaj pamti relevantnu verziju pravila dodele; kasnija promena ne menja potajno već odobren tok. | ASN-03, ATT-01 |
| ASN-09 | J | Kontrolisano otkazivanje dodele | Ovlašćena osoba otkazuje dodelu uz razlog; novi pokušaji nisu mogući, a postojeća istorija nije obrisana. | ASN-01, ATT-01 |
| ASN-10 | J | Obrazloženje dostupnosti studentu | Student vidi da li je vežba dostupna i osnovni razlog zabrane, bez prikazivanja skrivenih pravila drugih grupa. | ASN-02, ASN-03 |
| ASN-11 | P | Ponavljajuće dodele po rasporedu | Nedeljni raspored pravi jedinstvene instance dodele; ponovljeno pokretanje rasporeda ne stvara duplikate. | ASN-01, ASN-03 |
| ASN-12 | P | Paketno zadavanje laboratorija grupama | Nastavnik pregleda ciljnu listu i konflikte pre potvrde; svaka uspešna dodela ostaje zasebno proverljiva. | ASN-05 |
| ASN-13 | P | Nastavnički pregled preduslova pre zadavanja | Pre dodele se prijavljuju neispunjivi preduslovi ili nedostupna revizija; upozorenje ne otkriva privatne podatke studenata. | ASN-07, REV-02 |
| ASN-14 | N | Determinističko razrešavanje sukobljenih dodela | Za više važećih dodela iste laboratorije postoji dokumentovana politika prioriteta; server je primenjuje dosledno. | ASN-01, ASN-06 |

### ATT - 9 novih (J 4, P 3, N 2)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| ATT-09 | J | Serverska validacija studentskog odgovora | Pre čuvanja proveravaju se šema, dozvoljeni tip i veličina odgovora; odbijen unos ne menja stanje pokušaja. | ATT-02, AUT-03 |
| ATT-10 | P | Istorija verzija radnih odgovora | Student pre konačne predaje vraća dozvoljenu stariju verziju; posle predaje istorijski zapis ostaje nepromenljiv. | ATT-02, ATT-04 |
| ATT-11 | J | Dnevnik tačaka čuvanja pokušaja | Svako prihvaćeno čuvanje beleži vreme i verziju stanja; ponovljen identičan zahtev ne pravi novi zapis. | ATT-02, ATT-05 |
| ATT-12 | J | Atomska provera prava pri predaji | Predaja i provera roka, članstva i stanja sprovode se kao jedna celina; paralelni zabranjeni zahtev ne prolazi. | ATT-04, ASN-03, ACC-04 |
| ATT-13 | J | Oporavak od prekida mreže pri čuvanju | Ponovljeno slanje sa istim ključem ne pravi duplikat niti vraća stanje unazad; klijent dobija važeću verziju. | ATT-01, ATT-05 |
| ATT-14 | P | Kontrolisana pauza i nastavak pokušaja | Pauza je dozvoljena samo kada pravila dodele to dopuštaju; server i dalje proverava ukupni rok. | ATT-06, ATT-07 |
| ATT-15 | P | Prilozi uz studentski pokušaj | Samo dozvoljeni tipovi i veličine datoteka mogu se vezati za sopstveni pokušaj; tuđ prilog nije dostupan. | ATT-03, SEC-06 |
| ATT-16 | N | Usklađivanje vanmrežnih izmena | Konfliktne lokalne izmene prikazuju razliku i zahtevaju izbor; ne postoji tiho prepisivanje novije verzije. | ATT-05, ATT-13 |
| ATT-17 | N | Zaštita toka pokušaja od naleta zahteva | Ograničenje opterećenja čuva prihvaćene izmene i dosledno odbija ili odlaže prekoračenja uz proverljiv status. | ATT-01, ATT-02 |

### EVAL - 9 novih (J 4, P 3, N 2)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| EVAL-09 | J | Kontrolisano ponovno pokretanje tehnički neuspele evaluacije | Samo evaluacija sa tehničkom greškom može se ponoviti po pravilima; originalna predaja i istorija ostaju očuvani. | EVAL-05, EVAL-06 |
| EVAL-10 | P | Objašnjiva povratna informacija po kriterijumima | Student vidi obrazloženje dozvoljenih kriterijuma, ali ne i skriveni ključ, interne reference ili odgovore unapred. | EVAL-03, EVAL-07 |
| EVAL-11 | J | Validacija odgovora evaluatora | Rezultat sa pogrešnom šemom ili nedozvoljenim vrednostima označava se tehničkom greškom i ne proglašava padom. | EVAL-04, EVAL-06 |
| EVAL-12 | J | Bezbedne javne poruke evaluacionih grešaka | Interna greška mapira se na stabilnu javnu šifru bez tajni ili putanja; detalji su dostupni samo ovlašćenima. | EVAL-03, AUD-03 |
| EVAL-13 | J | Trajni zbirni rezultat po verziji | Sačuvani zbirni rezultat jednoznačno navodi evaluator verziju, maksimum i konačni ishod bez menjanja starijih rezultata. | EVAL-04, EVAL-05 |
| EVAL-14 | P | Probni paket test-odgovora za evaluatora | Nastavnik pokreće skup kontrolnih primera bez stvarnih studentskih pokušaja i dobija izveštaj prolaza i padova. | EVAL-01, AUT-15 |
| EVAL-15 | P | Uporedni prikaz rezultata ponovnih evaluacija | Dozvoljeni pregled jasno prikazuje razlike između verzija rezultata; raniji rezultat se ne briše. | EVAL-05 |
| EVAL-16 | N | Složena evaluacija sa više podprovera | Više evaluator koraka izvršava se po definisanom redosledu; neuspeh koraka ne proizvodi proizvoljan konačni rezultat. | EVAL-01, EVAL-04 |
| EVAL-17 | N | Paketna ponovna evaluacija uz nadzor | Ovlašćeni nastavnik najpre pregleda uticaj i potvrđuje proces; svaka promena čuva prethodni rezultat i reviziju. | EVAL-05, REV-03 |

### REVW - 8 novih (J 4, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| REVW-08 | J | Red čekanja za ručni pregled | Nastavnik vidi samo pokušaje svojih kurseva koji zahtevaju pregled, sa jasnim statusom i redosledom. | REVW-01, EVAL-04 |
| REVW-09 | P | Komentari uz konkretne korake rešenja | Komentar se vezuje za identifikator koraka i čuva istoriju izmena; student vidi samo dozvoljene komentare. | REVW-02, AUT-02 |
| REVW-10 | J | Preuzimanje pojedinačnog zadatka pregleda | Jedan aktivan pregled ima vlasnika i kontrolu konkurentnih izmena; druga osoba ne može tiho prepisati rad. | REVW-01 |
| REVW-11 | J | Strukturirane oznake povratne informacije | Nastavnik bira oznake prema dozvoljenom rečniku; studentski prikaz ne otkriva skrivena evaluaciona pravila. | REVW-02, REVW-04 |
| REVW-12 | J | Zaključenje ručnog pregleda uz razlog | Konačna odluka beleži nastavnika, vreme, razlog i prethodni status; ne može se zaključiti tuđ pokušaj. | REVW-03, ACC-04 |
| REVW-13 | P | Šabloni nastavničkih komentara | Nastavnik održava privatne šablone; primena pravi zaseban komentar koji se može proveriti i urediti. | REVW-02 |
| REVW-14 | P | Označavanje sumnjivih pokušaja za naknadni pregled | Ovlašćena osoba označava konkretne pokušaje uz razlog; sama oznaka ne menja automatski ocenu. | REVW-01, AUD-01 |
| REVW-15 | N | Arbitraža nesaglasnih ocena | Kod različitih nezavisnih ocena sistem zahteva zasebnu odluku arbitra i čuva sve prethodne procene. | REVW-07 |

### PRG - 8 novih (J 4, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| PRG-08 | P | Pregled ostvarenih kompetencija | Student vidi vlastitu mapu kompetencija izvedenu iz završenih laboratorija, bez pristupa tuđim rezultatima. | PRG-01, PRG-04 |
| PRG-09 | J | Sažetak napretka po kursu | Brojevi dodeljenih, započetih i završenih vežbi računaju se samo za korisnikov kurs i ne broje duplikate. | PRG-01, ASN-02 |
| PRG-10 | J | Prikaz rezultata u odnosu na maksimalan broj poena | Prikaz koristi maksimum i pravila iz odgovarajuće revizije; različite verzije se ne sabiraju pogrešno. | PRG-03, EVAL-13 |
| PRG-11 | J | Detaljni prikaz pokušaja iz pregleda napretka | Korisnik otvara samo sopstveni pokušaj; nastavnička prava se proveravaju zasebno prema kursu. | PRG-01, ATT-03 |
| PRG-12 | J | Status napretka po promeni članstva | Posle ispisa nema pristupa zaštićenom prikazu kursa, dok istorijski podaci ostaju sačuvani za dozvoljene uloge. | PRG-01, ACC-11 |
| PRG-13 | P | Periodični lični sažetak napretka | Korisnik može uključiti ili isključiti periodični sažetak; poruka ne uključuje rezultate drugih studenata. | PRG-09, AUD-05 |
| PRG-14 | P | Milestones i postignuća za završene celine | Postignuće se dodeljuje prema proverljivom pravilu i ne duplira se ponovljenim događajem. | PRG-01, INT-02 |
| PRG-15 | N | Procena završetka kursa uz objašnjenje | Prognoza prikazuje osnovu procene i oznaku nesigurnosti; mali uzorci i isključena personalizacija ne daju prognozu. | PRG-06, PRG-07 |

### AUD - 7 novih (J 3, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| AUD-08 | N | Provera integriteta revizorskih zapisa | Naknadno izmenjen ili nedostajući zapis otkriva se proverom kontrolnih otisaka; provera ne otkriva tajne. | AUD-01, AUD-02 |
| AUD-09 | J | Registar tipova revizorskih događaja | Svaki bezbednosno značajan događaj ima jedinstvenu šifru i obavezna polja; nepoznata šema se odbija. | AUD-01, AUD-02 |
| AUD-10 | J | Nadzor isporuke revizorskih događaja | Neuspeo upis se evidentira kao tehnička greška; sistem ne prijavljuje lažnu uspešnost kada je audit obavezan. | AUD-01, AUD-04 |
| AUD-11 | J | Kontrolisano pretraživanje revizorske istorije | Pretraga koristi ograničen vremenski opseg i prava korisnika; ne otkriva događaje iz nedozvoljenih kurseva. | AUD-01, ACC-04 |
| AUD-12 | P | Zahtev za proveru sumnjivog događaja | Ovlašćeni nastavnik otvara predmet provere sa stanjem i razlogom; automatska promena studentske ocene nije dozvoljena. | AUD-06 |
| AUD-13 | P | Merenje kašnjenja audit obrade | Administrator vidi agregatno kašnjenje i broj grešaka, ali ne i tajne ili sirove studentske odgovore. | AUD-10 |
| AUD-14 | P | Pravila za obaveštenja o audit događajima | Ovlašćena osoba podešava jednostavna pravila po tipu događaja; ponovljen događaj ne stvara nekontrolisane duplikate. | AUD-05, AUD-09 |

### ADM - 7 novih (J 3, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| ADM-09 | J | Režim održavanja sa kontrolisanim pristupom | U režimu održavanja upisne radnje su blokirane, dok zdravstvene provere i dozvoljeni administratorski tokovi rade. | ADM-04 |
| ADM-10 | J | Bezbedni demonstracioni podaci | Komanda za seed stvara isključivo sintetičke naloge i zadatke; ponovljeno pokretanje je predvidivo i bez duplikata. | ADM-02 |
| ADM-11 | J | Evidencija dostupnih rezervnih kopija | Svaka kopija ima vreme, status i proveru ispravnosti; nedostupna kopija se ne prikazuje kao upotrebljiva. | ADM-03 |
| ADM-12 | P | Pregled razlike između konfiguracija | Administrator dobija prikaz promenjenih bezbednih parametara pre potvrde; tajne vrednosti su maskirane. | ADM-04 |
| ADM-13 | P | Administratorski panel stanja sistema | Ovlašćeni korisnik vidi stanje procesa i baze uz ograničene detalje; javni API ne prikazuje internu konfiguraciju. | OPS-10, ACC-04 |
| ADM-14 | P | Probni pregled pravila zadržavanja podataka | Pre izvršenja politike vidi se broj i tip pogođenih stavki; probni pregled ne briše ništa. | ADM-05 |
| ADM-15 | N | Politike privatnosti po kursu i ustanovi | Izvoz i čuvanje podataka proveravaju se prema kontekstu ustanove i kursa, bez zaobilaženja osnovnih prava. | ADM-05, ADM-08 |

### TYP - 8 novih (J 4, P 3, N 1)

| ID | Nivo | Nova funkcionalnost | Minimalni kriterijum prihvatanja | Veze |
|:--|:--:|:--|:--|:--|
| TYP-09 | J | Laboratorija višestrukog izbora | Tačan izbor se proverava na serveru i nije poslat klijentu unapred; pogrešan odgovor daje proverljiv ishod. | EVAL-01, AUT-04 |
| TYP-10 | J | Laboratorija za redosled koraka incidenta | Student slaže korake; ocenjivanje koristi objavljeni redosled i deterministički obrazložen rezultat. | EVAL-02 |
| TYP-11 | J | Laboratorija za klasifikaciju bezbednosnih događaja | Student klasifikuje sintetičke zapise prema kategorijama; nepoznata kategorija ili nevažeći format se odbijaju. | TYP-02, EVAL-01 |
| TYP-12 | J | Laboratorija povezivanja imovine i zaštitnih kontrola | Student uparuje imovinu sa merama zaštite; više validnih parova ocenjuje se prema eksplicitnom ključu. | EVAL-01 |
| TYP-13 | P | Simulacija analize zaglavlja poruke | Koriste se lokalni sintetički primeri, bez slanja poruka ili kontakta sa spoljnim adresama; nalaz ima objašnjenje. | TYP-11 |
| TYP-14 | P | Scenariji izbora kriptografskih parametara | Student bira sigurnu konfiguraciju iz unapred zadatih opcija; ne implementiraju se sopstveni kriptografski algoritmi. | TYP-03 |
| TYP-15 | P | Simulacija prioritizacije ranjivosti zavisnosti | Student klasifikuje sintetičke nalaze prema riziku i kontekstu; rešenje daje obrazložen prioritet. | TYP-03 |
| TYP-16 | N | Razgranati laboratorijski scenario | Izbor studenta aktivira sledeći definisani korak; ista verzija scenarija i isti odgovori daju isti tok i rezultat. | AUT-02, EVAL-02 |

## Napomena za postojeće prijave

Dodeljene oznake ostaju rezervisane prema aktuelnoj tabeli, bez automatske izmene. Pre nove dodele proveriti preostale slobodne J/P/N oznake, minimalni broj članova i timove sa više od jedne J. Prioritet razvoja dati ATT, EVAL i REVW jezgru, kako bi se ostvarili osnovni studentski tokovi pre izgradnje složenih N funkcionalnosti.

## Održavanje specifikacije

Ovaj dokument dopunjuje postojeći katalog. Prilikom naredne konsolidacije preneti novih 98 redova u jedinstvenu DOCX/PDF specifikaciju, uz osvežavanje zbirnog broja tema i sadržaja. Do tada, **nova oznaka i njen kriterijum su definisani ovde**, a postojeći opšti zahtevi ostaju na snazi.
