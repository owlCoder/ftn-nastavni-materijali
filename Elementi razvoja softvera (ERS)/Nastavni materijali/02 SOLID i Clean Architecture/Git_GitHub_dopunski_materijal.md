# Git i GitHub — dopunski materijal

Git je distribuirani sistem za kontrolu verzija. GitHub je platforma za hostovanje repozitorijuma i saradnju.

## Lokalni rad

- git init — inicijalizacija
- git clone URL — kopija repozitorijuma
- git status — pregled stanja
- git add . — staging
- git commit -m "Opis" — lokalni commit
- git log --oneline — istorija

## Saradnja

- git fetch — preuzimanje izmenjenih referenci
- git merge grana — integracija
- git pull — preuzimanje i integracija
- git push -u origin grana — objavljivanje
- git switch -c grana — nova grana
- git commit --amend — izmena poslednjeg commit-a
- git rebase — preuređivanje istorije

GitHub Desktop i Visual Studio nude odgovarajuće operacije u grafičkom interfejsu. Za .NET koristiti .gitignore za bin/, obj/ i .vs/.

Praktični zadatak: napraviti privatni repozitorijum ers_XXX_tim_YY, dodati članove tima i asistenta. Zatim vežbati grane, commit, push i Pull Request.

Izvor: PDF „02 Kontrola verzije softvera. Git. GitHub.pdf” i odgovarajući Canva dizajn od 21 strane.
