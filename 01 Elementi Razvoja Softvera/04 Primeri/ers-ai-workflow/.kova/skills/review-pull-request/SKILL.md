---
name: review-pull-request
description: Review an ERS change against requirements, architecture and observed test results.
---

# review-pull-request

## Svrha
Pregled jedne promene u odnosu na zahtev, Clean Architecture, SOLID i testove.

## Režim rada
Koristi se u Kova režimu **Manual**: čitanje radi direktno, a poziv `run_unit_tests` traži odobrenje.

## Ulazi
- user story / issue i kriterijumi prihvatanja
- projektna pravila iz `AGENTS.md`
- `git diff` (MCP alat `get_git_diff`)
- rezultat testova (MCP alat `run_unit_tests`)

## Postupak
1. Pročitaj `AGENTS.md`.
2. Sažmi očekivano ponašanje bez izmišljanja zahteva.
3. Proveri da li diff izlazi iz obima stavke.
4. Proveri smer zavisnosti: Domain ← Application ← Infrastructure/Presentation.
5. Proveri da li poslovna pravila cure u API/Console/MCP/hook sloj.
6. Proveri negativne i granične scenarije.
7. Uporedi promenjeno ponašanje sa testovima.
8. Vrati nalaze po ozbiljnosti i navedi dokaz.

## Izlaz
- `blockingFindings`
- `nonBlockingFindings`
- `missingTests`
- `architectureNotes`
- `verificationEvidence`

## Ograničenja
- Ne menjaj kod.
- Ne predlaži potpuni rewrite kada je mala lokalna izmena dovoljna.
- Ne proglašavaj testove uspešnim bez stvarnog izlaza test alata.
