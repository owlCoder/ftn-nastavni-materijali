---
name: implement-approved-plan
description: Implement a previously approved ERS change plan, keep layer boundaries and verify with real commands.
---

# implement-approved-plan

## Svrha
Implementacija plana koji je tim prethodno usvojio, bez proširivanja zahteva.

## Režim rada
Koristi se u Kova režimu **Manual**: svaka izmena datoteke i svaka komanda traže odobrenje pre izvršenja.

## Ulazi
- usvojen plan iz skill-a `architecture-review`
- projektna pravila iz `.ai/AI_INSTRUCTIONS.md`
- datoteke navedene u planu i njihovi testovi

## Postupak
1. Pročitaj `.ai/AI_INSTRUCTIONS.md` i usvojen plan.
2. Menjaj samo datoteke navedene u planu; ako je potrebna još neka, stani i prijavi.
3. Poslovno pravilo drži u Domain/Application delu, van API, Console i MCP sloja.
4. Koristi postojeće portove ili uvedi uzak novi port kada je potrebna spoljašnja zavisnost.
5. Za svako promenjeno ponašanje dodaj ili izmeni test.
6. Pokreni testove (MCP alat `run_unit_tests`) i pregledaj diff (MCP alat `get_git_diff`).

## Izlaz
- `summary`
- `changedFiles`
- `executedCommands`
- `testResult`
- `openQuestions`

## Ograničenja
- Ne uvodi nove zahteve.
- Ne menjaj očekivanje testa da bi test prošao.
- Ne tvrdi da je nešto provereno ako stvarna komanda nije izvršena.
- Ne čitaj `.env`, tajne ili pristupne tokene.
