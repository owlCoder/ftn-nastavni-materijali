# Osnove informacione bezbednosti — Kontrolne tačke

## KT1 — Osnovna implementacija i bezbednosni model

Termin: Vežba 4

Prva kontrolna tačka održava se u terminu 4. vežbe. Tim prikazuje obuhvat svih dodeljenih celina, bezbednosni model i prvi zaokružen funkcionalni tok sa serverskom zaštitom i negativnim testom.

### Obavezno za sve timove

1. Za SVAKU dodeljenu celinu: lista use-case-ova iz specifikacije, redosled implementacije i označeni preduslovi (TAPIZ backlog).
2. Jedan objedinjeni domen model / skica toka, ključni entiteti, statusi i vlasništvo podataka; minimalna security matrica Subject–Action–Resource–Allow/Deny za relevantne operacije.
3. Repozitorijum: osnovna struktura aplikacije, README sa startovanjem, build koji prolazi, početni CI i Tapiz task → branch → PR praksa.
4. Primarna celina: najmanje jedan zaokružen funkcionalan tok koji prolazi kroz aplikacionu logiku i persistence/simulator, sa demonstracijom normalnog ishoda; za jednostavnije celine očekuju se bar dve ključne operacije (npr. unos + promena statusa).
5. Serverska zaštita konkretne operacije (scope/ownership/role prema domenu), validacija ulaza i jedan AUTOMATIZOVAN negativni test; ne smatra se dovoljnom zaštita samo kroz UI.
6. Threat model svih dodeljenih celina: asset → misuse/pretnja → kontrola → test → evidence, uz prikaz bar jedne trust granice.
7. Migracije/seed ili ekvivalentno ponovljivo testno stanje; strukturisani događaj/audit za relevantnu promenu bez tajni.
8. Minimalni cross-team ugovor pregledan sa susednim timom; ako implementacija nije spremna, stub + pozitivni i negativni odgovor.

### Prikaz

Model i backlog, pokrenut normalan scenario, negativan automatski test i konkretan plan završetka do KT2 u terminu 8. vežbe.

### Ne tražiti u ovoj fazi

- Puna realizacija druge/treće celine.
- Produkcioni simulator.

## KT2 — Završna demonstracija i security QA

Termin: Vežba 8

Druga kontrolna tačka održava se u terminu 8. vežbe. Tim demonstrira sve obavezne use-case-ove svih dodeljenih celina u odobrenom obimu, njihovo povezivanje, bezbednosne odluke i ponovljivo pokretanje, uz ažurnu dokumentaciju.

### Obavezno za sve timove

1. Svi obavezni use-case-ovi AKTIVNOG/ODOBRENOG scope-a celina; ako se puni normativni scope iz SCUTUM specifikacije smanjuje, NASTAVNI TIM mora to unapred eksplicitno objaviti kao reviziju.
2. End-to-end demo: početno stanje → akcija → odluka dozvoli/odbij → relevantan security/audit trag → konačan status.
3. Unit + negativni security + odgovarajući integracioni/contract testovi po relevantnoj celini, CI green, dokumentovane revizije PR.
4. Clean clone, migracije i seed/demo podaci rade prema README-u.
5. API/event/policy ugovori, threat model, ADR, evidencija testiranja, individualni doprinosi i residual risk su ažurni.
6. Cross-team integracija sa realnim adapterom gde je dostupna; samo stub ne prikazivati kao završenu realnu integraciju.

### Dodatno za timove sa 3 celine

7. Treća celina mora imati javno odobren opseg i izvršiv bezbednosni tok sa automatizovanim testovima; kompletan normativni obim je obavezan osim ako je nastavni tim formalno promenio specifikaciju.

### Prikaz

Povezani end-to-end scenario, pozitivan i negativan test, ugovori i individualno obrazloženje rada. Kontrolne tačke i termini odbrane projekta su odvojene obaveze.
