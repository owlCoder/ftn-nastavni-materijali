---
name: architecture-review
description: Analyze how a requested ERS change affects layers and module boundaries, without editing files.
---

# architecture-review

## Svrha
Analiza uticaja jedne promene na slojeve i granice sistema pre nego što implementacija počne.

## Režim rada
Koristi se u Kova režimu **Plan**: dostupni su samo alati za čitanje, pa izmena datoteka i pokretanje komandi nisu mogući.

## Ulazi
- zahtev i kriterijumi prihvatanja
- projektna pravila iz `AGENTS.md` (Kova ih učitava automatski)
- struktura projekta (MCP alat `get_project_structure`)
- trenutni `git diff` (MCP alat `get_git_diff`)
- relevantan kod i testovi

## Postupak
1. Sažmi zahtev bez dodavanja novih pravila; ako pravilo nije definisano, traži pojašnjenje.
2. Navedi pogođene projekte i slojeve.
3. Odredi gde pravilo pripada: validator, domenski servis, use-case ili adapter.
4. Proveri da plan ne menja smer zavisnosti: Domain ← Application ← Infrastructure/Presentation.
5. Predloži mali plan izmene po datotekama.
6. Navedi test scenarije: uspešan, negativan i granični.

## Izlaz
- `affectedLayers`
- `assumptions`
- `risks`
- `plan`
- `testScenarios`

## Ograničenja
- Ne menjaj datoteke.
- Ne proširuj poslovni zahtev.
- Ne predlaži potpuni rewrite kada je mala lokalna izmena dovoljna.
