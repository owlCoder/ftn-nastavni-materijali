# Osnove informacione bezbednosti — Kontrolne tačke

## KT1 — Scope, model i pokrenut projekat

Termin: Kraj 2. nedelje

Tim utvrđuje obuhvat svih dodeljenih celina, zajednički model i početne ugovore, pokreće projekat i pokazuje prvo izvršivo pravilo.

### Obavezno za sve timove

1. Za SVAKU dodeljenu celinu: lista use-case-ova iz specifikacije, redosled implementacije i označeni preduslovi (TAPIZ backlog).
2. Jedan objedinjeni domen model / skica toka, ključni entiteti, statusi i vlasništvo podataka; minimalna security matrica Subject–Action–Resource–Allow/Deny za relevantne operacije.
3. Prva mapa međutimskih zavisnosti: owner, consumer, minimalni API/event/policy ugovor u DRAFT stanju (ako postoji granica).
4. Repozitorijum: osnovna struktura aplikacije, README sa startovanjem, build koji prolazi, početni CI i Tapiz task → branch → PR praksa.
5. Jedno izvršivo pravilo ili minimalni test domenske invarijante.

### Prikaz

5–7 minuta; model, backlog, dokaz da se projekat build-uje, jedan test i konkretan plan KT2.

### Ne tražiti u ovoj fazi

- Završen MFA.
- Detections engine.
- Full frontend.
- Finalne integracije.

## KT2 — Prvi vertikalni scenario

Termin: Kraj 4. nedelje

Primarna celina ima prvi zaokružen funkcionalan tok, serversku zaštitu i negativan automatski test. Model pretnji obuhvata sve dodeljene celine.

### Obavezno za sve timove

1. Primarna celina: najmanje jedan zaokružen funkcionalan tok koji prolazi kroz aplikacionu logiku i persistence/simulator, sa demonstracijom normalnog ishoda; za jednostavnije celine očekuju se bar dve ključne operacije (npr. unos + promena statusa).
2. Serverska zaštita konkretne operacije (scope/ownership/role prema domenu), validacija ulaza i jedan AUTOMATIZOVAN negativni test; ne smatra se dovoljnom zaštita samo kroz UI.
3. Threat model svih dodeljenih celina: asset → misuse/pretnja → kontrola → test → evidence, uz prikaz bar jedne trust granice.
4. Migracije/seed ili ekvivalentno ponovljivo testno stanje; strukturisani događaj/audit za relevantnu promenu bez tajni.
5. Minimalni cross-team ugovor pregledan sa susednim timom; ako implementacija nije spremna, stub + pozitivni i negativni odgovor.

### Prikaz

Pokrenut normalan scenario i negativan automatski test.

### Ne tražiti u ovoj fazi

- Puna realizacija druge/treće celine.
- Produkcioni simulator.

## KT3 — Druga celina i povezivanje

Termin: Kraj 6. nedelje

Prva i druga dodeljena celina imaju izvršive tokove, provereno povezivanje i ugovor gde postoji međutimska zavisnost. Timovi sa tri celine prikazuju minimalni izvršiv prototip treće celine.

### Obavezno za sve timove

1. Izvršiv ključni funkcionalni tok iz PRVE i DRUGE dodeljene celine i način na koji su povezane (ili legitimno nezavisne, ali sa jasnim zajedničkim bezbednosnim scenarijem).
2. Pozitivan + negativan automatski test toka; odgovarajući audit event, kontrolisana greška i prolazak kroz CI.
3. Bar jedan dogovoreni među-timski contract/security test gde zavisnost postoji; stubbing privremeno dozvoljen uz dokaz ugovora.
4. Dokumentovana ograničenja i plan prelaska sa stub-a na adapter.

### Dodatno za timove sa 3 celine

5. Treća celina ima izvršiv prototip JEDNOG ključnog use-case-a/pravila/odluke; ne traži se ceo R3 podsistem u ovom trenutku.

### Prikaz

Jedan povezani scenario, negative test i ugovor sa drugim timom.

## KT4 — Finalna demonstracija i security QA

Termin: Kraj 8. nedelje

Tim demonstrira sve obavezne use-case-ove aktivnog/odobrenog scope-a, dokazuje bezbednosne odluke i ponovljivo pokretanje i predaje ažurnu dokumentaciju. Normativni obim ostaje obavezan dok nastavni tim ne objavi formalnu reviziju.

### Obavezno za sve timove

1. Svi obavezni use-case-ovi AKTIVNOG/ODOBRENOG scope-a celina; ako se puni normativni scope iz SCUTUM specifikacije smanjuje, NASTAVNI TIM mora to unapred eksplicitno objaviti kao reviziju.
2. End-to-end demo: početno stanje → akcija → odluka dozvoli/odbij → relevantan security/audit trag → konačan status.
3. Unit + negativni security + odgovarajući integracioni/contract testovi po relevantnoj celini, CI green, dokumentovane revizije PR.
4. Clean clone, migracije i seed/demo podaci rade prema README-u.
5. API/event/policy ugovori, threat model, ADR, evidencija testiranja, individualni doprinosi i residual risk su ažurni.
6. Cross-team integracija sa realnim adapterom gde je dostupna; samo stub ne prikazivati kao završenu realnu integraciju.

### Dodatno za timove sa 3 celine

7. Treća celina mora imati javno odobren opseg i izvršiv bezbednosni tok sa automatizovanim testovima; kompletan normativni obim je obavezan osim ako je nastavni tim formalno promenio specifikaciju.
