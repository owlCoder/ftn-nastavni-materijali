# TAPIZ BOARDS

## Упутство за постављање и коришћење заједничког радног окружења

*Тимски рад, управљање задацима и заједничко развојно окружење*

## Увод

Tapiz Boards представља централно радно окружење за планирање, праћење и документовање тимског рада на софтверским пројектима. Заједничка инстанца омогућава свим члановима тима приступ истом скупу пројектних података, уз јасно разграничене корисничке и инфраструктурне привилегије.

Препоручена поставка заснива се на томе да Vercel извршава апликацију, Aiven обезбеђује MySQL базу података, а чланови тима приступају систему преко заједничког production URL-а и сопствених Tapiz корисничких налога. Приступ инфраструктурним сервисима додељује се искључиво лицима задуженим за њихово одржавање.

> Референтни репозиторијум: owlCoder/tapiz-boards-oss. Техничке кораке потребно је ускладити са структуром и конфигурацијом актуелне верзије репозиторијума.

## 1. Архитектура и модел приступа

Tapiz Boards OSS је Next.js апликација са MySQL базом података и сопственим email/password механизмом за пријаву. У препорученом моделу Vercel хостује апликацију, док Aiven хостује базу података. Иницијализацију шеме и првог административног налога обавља једна овлашћена особа.

| Компонента | Улога у систему | Приступ |
| --- | --- | --- |
| Члан тима | Користи board, backlog, story-је и остале функције система. | Tapiz налог и production URL. |
| Vercel | Извршава Next.js апликацију и обезбеђује production URL. | Приступ само лицима задуженим за deployment. |
| Aiven | Обезбеђује централну MySQL базу и TLS везу. | Приступ само лицима задуженим за базу и инфраструктуру. |

### 1.1. Додела инфраструктурних привилегија

| Улога | Потребан приступ | Приступ који није потребан |
| --- | --- | --- |
| Обичан члан тима | Tapiz налог и production URL. | Aiven налог, лозинка базе и Vercel административни приступ. |
| Лице задужено за репозиторијум | GitHub write access; по потреби Vercel project/team приступ. | Директан приступ бази ако не обавља DB операције. |
| Лице задужено за deployment и базу | Vercel settings и Aiven project/service приступ. | Дељење приступних података преко порука, докумената или репозиторијума. |

> Принцип приступа: Сваки корисник треба да добије само онај ниво приступа који је неопходан за његову улогу. Коришћење board-а не захтева Vercel или Aiven налог.

### 1.2. Техничке могућности референтног репозиторијума

- Next.js 16 и React 19, MySQL и Drizzle ORM.
- Auth.js пријава путем email адресе и лозинке, без public self-registration механизма.
- Администратор креира кориснике кроз /admin/users.
- Serverless-safe DB pool; подразумевани pool limit је 1.
- Aiven TLS CA може се проследити кроз DATABASE_SSL_CA_BASE64.
- Endpoint /api/health проверава стање сервиса и везу ка бази.
## 2. Постављање MySQL базе на Aiven сервису

### 1. Креирање Aiven MySQL сервиса

У Aiven Console потребно је формирати project и у њему Aiven for MySQL сервис. За почетну поставку препоручује се најмањи план који задовољава потребе тима; план се касније може променити ако оптерећење то захтева.

### 2. Креирање базе tapiz_boards

У оквиру сервиса, у делу Connect → Databases → Create database, потребно је креирати базу tapiz_boards. Технички је могуће користити и defaultdb, али је засебна база прегледнија за ову намену.

### 3. Преузимање Service URI вредности

MySQL connection URI треба копирати директно из Aiven конзоле. Ручно састављање URI вредности није препоручљиво, јер лозинка може садржати знакове који захтевају URL encoding.

```text
mysql://avnadmin:********@HOST:PORT/tapiz_boards
```

### 4. Преузимање CA сертификата

Ако је у делу Connection information доступна опција CA Certificate → Download, потребно је преузети датотеку ca.pem. У зависности од конфигурације сервиса, она се користи за проверу TLS везе ка бази.

### 2.1. Претварање CA сертификата у Base64 формат

Вредност сертификата може се претворити у једну Base64 линију и проследити апликацији као environment variable.

Linux / macOS:

```text
base64 < ca.pem | tr -d '\n'
```

PowerShell:

```text
[Convert]::ToBase64String([IO.File]::ReadAllBytes("ca.pem"))
```

> Безбедносна напомена: Service URI, лозинка базе, ca.pem, Base64 CA и AUTH_SECRET не смеју се уписивати у Git репозиторијум. Ове вредности чувају се у локалној .env датотеци и у Vercel Environment Variables.

### 2.2. Препоручени приступ Aiven сервису

За иницијално постављање могу се користити приступни подаци које Aiven генерише уз сервис. За дугорочно одржавање препоручује се посебан service user за апликацију и додела Aiven project permissions само лицима којима је тај приступ неопходан.

## 3. Једнократна иницијализација базе

Иницијализацију базе обавља једна овлашћена особа са локалног рачунара. Vercel build не покреће миграције аутоматски, чиме се измена шеме задржава као експлицитна и контролисана операција.

### 3.1. Преузимање репозиторијума и инсталација зависности

```text
git clone https://github.com/owlCoder/tapiz-boards-oss.git
cd tapiz-boards-oss
npm install
cp .env.example .env
```

На Windows систему .env.example може се копирати у .env коришћењем Explorer-а или развојног окружења.

### 3.2. Локална .env конфигурација

```text
DATABASE_URL=mysql://avnadmin:...@HOST:PORT/tapiz_boards
DATABASE_SSL_CA_BASE64=<BASE64_OD_CA_PEM>
AUTH_SECRET=<GENERISANI_SECRET>
AUTH_TRUST_HOST=true
DATABASE_POOL_LIMIT=1

# За локалну иницијализацију AUTH_URL није потребан.
# У Vercel окружењу користиће се production URL.
```

Auth.js secret може се генерисати следећом командом:

```text
openssl rand -base64 32
```

Ако OpenSSL није доступан, потребно је употребити други поуздан алат за генерисање најмање 32 криптографски случајна бајта и резултат сачувати као secret.

### 3.3. Креирање шеме базе

Референтни репозиторијум користи drizzle-kit push. Пошто drizzle.config.ts чита DATABASE_URL, а не посебну CA променљиву, за овај једнократни CLI корак може се употребити привремена URI вредност са SSL опцијом.

Linux / macOS:

```text
DATABASE_URL='mysql://avnadmin:...@HOST:PORT/tapiz_boards?ssl={"rejectUnauthorized":false}' \
  npm run db:push
```

PowerShell:

```text
$env:DATABASE_URL='mysql://avnadmin:...@HOST:PORT/tapiz_boards?ssl={"rejectUnauthorized":false}'
npm run db:push
Remove-Item Env:DATABASE_URL
```

> Важно: Параметар rejectUnauthorized=false користи се само за наведени локални drizzle-kit push корак када је потребан. Таква URI вредност не поставља се у Vercel. Runtime апликације треба да користи стандардни DATABASE_URL и CA кроз DATABASE_SSL_CA_BASE64.

### 3.4. Креирање првог административног налога

```text
npm run create-admin -- \
  --email admin@vas-tim.rs \
  --password "<jaka-lozinka>" \
  --first-name Admin \
  --last-name Tima
```

Овај налог се користи за иницијалну администрацију и касније креирање корисничких налога за остале чланове тима.

## 4. Постављање апликације на Vercel

### 4.1. Увоз GitHub репозиторијума

У Vercel интерфејсу потребно је изабрати Add New → Project → Import Git Repository и одабрати репозиторијум owlCoder/tapiz-boards-oss.

### 4.2. Build подешавања

| Поље | Вредност |
| --- | --- |
| Framework Preset | Next.js (auto-detected) |
| Root Directory | . |
| Install Command | подразумевано / npm install |
| Build Command | подразумевано / npm run build |
| Output Directory | оставити подразумевану вредност |

### 4.3. Environment Variables

За прву поставку следеће вредности треба додати у Production scope. Продукциону базу није препоручљиво аутоматски делити са Preview deployment-има.

| Променљива | Вредност | Напомена |
| --- | --- | --- |
| DATABASE_URL | Aiven Service URI ка tapiz_boards | SECRET |
| DATABASE_SSL_CA_BASE64 | Base64 садржај ca.pem | ако сервис користи project CA |
| AUTH_SECRET | резултат openssl rand -base64 32 | SECRET |
| AUTH_TRUST_HOST | true | потребно иза Vercel proxy-ја |
| DATABASE_POOL_LIMIT | 1 | опционо; репозиторијум подразумева 1 |

Vercel подржава одвојене environment variables за Production, Preview и Development scope. Након измене environment variable вредности потребно је покренути нови deployment.

### 4.4. Први deployment и подешавање коначног URL-а

Након првог Deploy поступка потребно је преузети стабилан production domain, на пример https://tapiz-boards-team.vercel.app, и додати следеће вредности у Production scope:

```text
AUTH_URL=https://tapiz-boards-team.vercel.app
NEXT_PUBLIC_SITE_URL=https://tapiz-boards-team.vercel.app
```

После чувања вредности покреће се Redeploy. Ако се накнадно уведе custom domain, обе URL вредности треба заменити новим HTTPS доменом и поново покренути deployment.

> Напомена: Два deployment корака су практична зато што се коначни production domain најједноставније добија након првог deployment-а. Login треба проверити тек након подешавања коначног AUTH_URL и поновног deployment-а.

## 5. Кориснички приступ и рад тима

### 5.1. Доступност production окружења

Tapiz Boards има сопствени механизам пријаве, па production domain треба да буде доступан корисницима апликације без обавезног Vercel login-а. Ако обични чланови тима немају Vercel налоге, Deployment Protection не треба поставити на All Deployments за production домен.

> Препорука: За типичан ток рада може се користити Standard Protection: preview deployment-и остају заштићени, док production domain остаје доступан корисницима који се аутентификују у Tapiz систему.

### 5.2. Креирање корисничких налога

1. Отворити production URL.
1. Пријавити се иницијалним административним налогом.
1. Отворити User management (/admin/users).
1. Креирати налог за сваког члана тима користећи email адресу и привремену јаку лозинку.
1. Члану тима доставити само production URL и његове Tapiz приступне податке.
> Напомена: Community/OSS издање нема public sign-up страницу. Корисничке налоге креира администратор; одсуство самосталне регистрације представља очекивано понашање система.

### 5.3. Додавање корисника у пројекат или board

Након креирања налога, власник пројекта може додати чланове директно или користити project invite code, у зависности од изабраног тока у апликацији. Сви корисници приступају истој централној бази преко Vercel апликације.

### 5.4. Додела cloud приступа

| Активност корисника | Препоручени приступ |
| --- | --- |
| Користи story-је, backlog, sprint-ове и board | Само Tapiz налог. |
| Push-ује код | GitHub collaborator / write access. |
| Прати deployment логове или мења environment variables | Vercel project/team приступ. |
| Обавља backup, DB дијагностику или администрацију базе | Aiven project permissions по принципу најмањих привилегија. |

## 6. Верификација након постављања

Пре почетка редовног коришћења потребно је проверити следеће услове:

- Production URL се отвара у приватном/incognito прозору без Vercel login-а.
- /api/health враћа HTTP 200.
- Health JSON садржи status: operational и db: ok.
- Администратор може успешно да се пријави.
- Администратор може да креира новог корисника.
- Нови корисник може да се пријави са другог рачунара.
- Могуће је креирати пројекат и почетне колоне.
- Измена story-ја остаје сачувана након refresh-а.
### 6.1. Провера health endpoint-а

```text
https://VAS-DOMEN.vercel.app/api/health
```

Очекивани облик одговора:

```text
{
  "service": "tapiz-boards",
  "status": "operational",
  "db": "ok",
  "timestamp": "..."
}
```

### 6.2. Опциони smoke test

Ако локална .env конфигурација показује на исту Aiven базу, репозиторијум садржи и smoke test:

```text
npm run smoke
```

> Пре покретања: Потребно је прочитати README и проверити шта тест извршава. Према референтном упутству, тест креира и уклања сопствене throwaway податке, али га ипак треба покретати као контролисану операцију над познатом базом.

## 7. Дијагностика најчешћих проблема

| Симптом | Највероватнији узрок | Провера / поступак |
| --- | --- | --- |
| /api/health → 503; db: down | DB URI, TLS CA или Aiven сервис. | Проверити DATABASE_URL, CA Base64, статус Aiven сервиса и покренути redeploy. |
| Login враћа на погрешан URL или прави loop | AUTH_URL није production origin. | Проверити тачан HTTPS domain, AUTH_TRUST_HOST=true и покренути redeploy. |
| Vercel тражи сопствени login пре Tapiz пријаве | Deployment Protection је постављен на All Deployments. | Прећи на Standard Protection или одговарајућу exception политику. |
| db:push пријављује SSL/cert проблем | Drizzle CLI не користи app CA променљиву. | Користити једнократни привремени URI описан у одељку 3.3. |
| Превише MySQL конекција | Serverless concurrency и превисок pool. | Проверити DATABASE_POOL_LIMIT=1. |
| Корисник не налази опцију Register | Нема public registration механизма. | Администратор креира налог у User management делу. |
| После измене environment variable вредности нема промене | Активан је стари deployment. | Покренути Redeploy; измене нису ретроактивне за већ изграђен deployment. |

## 8. Безбедан и одржив модел рада

За типичан студентски тим није потребна сложена инфраструктура, али доследна примена ограниченог приступа и контролисаних измена значајно смањује оперативни ризик.

- Приступне податке за Aiven базу не треба делити обичним корисницима; они комуницирају искључиво са Vercel апликацијом.
- .env и ca.pem не смеју бити commit-овани у репозиторијум.
- Production secrets треба чувати у Production scope-у. Ако је потребно функционално Preview окружење, препоручује се посебна preview база и одвојени приступни подаци.
- Број лица са административним приступом Vercel и Aiven сервисима треба ограничити. Репозиторијум може имати више развојних чланова, али продукционе secrets треба да мења мали број овлашћених лица.
- Пре промене шеме базе препоручује се backup/export у складу са договореном оперативном политиком.
- У случају откривања secret вредности потребно је извршити ротацију; брисање поруке или Git commit-а није довољно.
- Након увођења custom domain-а потребно је ажурирати AUTH_URL и NEXT_PUBLIC_SITE_URL и покренути нови deployment.
### 8.1. Препоручена расподела одговорности

| Компонента | Препорука |
| --- | --- |
| GitHub | Развојни чланови имају одговарајући repository access; main грана се штити и измене се прегледају кроз Pull Request. |
| Vercel | Један production пројекат; Production secrets; контролисан приступ подешавањима. |
| Aiven | Један MySQL сервис и база tapiz_boards; мали број оператора. |
| Tapiz Boards | Један или неколико административних налога; остали корисници имају стандардне налоге. |
| Preview окружење | По потреби посебна база и одвојене secrets ако тим користи функционалне PR preview-е. |

> Препоручени оперативни ток: Код се развија у GitHub репозиторијуму → Vercel поставља main грану → Aiven остаје централна база → чланови тима приступају систему преко production URL-а и сопствених Tapiz налога.

## 9. Референтна документација

За подешавања која зависе од конкретне верзије апликације или спољног сервиса користити актуелну званичну документацију наведених платформи. При промени верзије потребно је проверити компатибилност конфигурационих параметара, безбедносних опција и поступка постављања.

Tapiz Boards репозиторијум: https://github.com/owlCoder/tapiz-boards-oss

Aiven - MySQL: https://aiven.io/docs/products/mysql/get-started

Aiven - креирање базе: https://aiven.io/docs/products/mysql/howto/create-database

Aiven - TLS/SSL сертификати: https://aiven.io/docs/platform/concepts/tls-ssl-certificates

Aiven - пројектне дозволе: https://aiven.io/docs/platform/howto/manage-permissions

Vercel - Environment Variables: https://vercel.com/docs/projects/environment-variables

Vercel - Deployment Protection: https://vercel.com/docs/security/deployment-protection
