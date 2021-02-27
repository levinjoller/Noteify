# Noteify

![CI only master](https://github.com/levinjoller/noteify/workflows/CI%20only/badge.svg)
![Deploy master](https://github.com/levinjoller/noteify/workflows/Deploy/badge.svg)

12.11.2020; BBZW-Sursee.\
This project was created as part of the technical school. The sensitive files have been removed in this new repository.

## Live demo

[https://m150noteify.herokuapp.com](https://m150noteify.herokuapp.com/)

## Build with

- Visual Studio Code
- ASP.NET Core 5.1
- ASP.NET Core Identity (Claim-based)
- PostgreSQL
- Microsoft OAuth 2.0 authorization (Over Azure)
- Docker (For CD)
- Heroku (Cloud Application Plattform; host)
- XUnit (Unit Tests)

## Installation

Install the latest [.Net SDK](https://dotnet.microsoft.com/download)\
Install the latest [PostgreSQL server](https://www.postgresql.org/download/)

The following commands must be executed on the /src/Noteify.Web/ directory.

### Postgres SQL DB

Add personal PostgreSQL connection informations

```sh
dotnet user-secrets set "DbUserId" "YourDbUserName"
```

```sh
dotnet user-secrets set "DbPassword" "YourDbPassword"
```

### Microsoft OAuth 2.0

```sh
dotnet user-secrets set "Authentication:Microsoft:ClientId" "YourClient-ID"
```

```sh
dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "YourClient-Secret"
```

## Usage

Navigate to the start project:

```sh
cd .\src\Noteify.Web\
```

Execute project:

```sh
dotnet run
```

Call up the application on:\
[https://localhost:5001](https://localhost:5001)\
or\
[http://localhost:5000](http://localhost:5000)

## Deployment

The following environment variables must be set on the corresponding Heroku account:

```sh
MICROSOFT_OAUTH_CLIENTID
```

```sh
MICROSOFT_OAUTH_CLIENTSECRET
```

```sh
DATABASE_URL
```

The preferred time zone:

```sh
TZ
```

For Microsoft OAuth 2.0, the corresponding URI of the website must be stored.

## To consider

The same Microsoft OAuth 2.0 account cannot be used for local and production authentication. In this exercise, the URI in the same Azure account was changed depending on the usage. I recommend two Azure accounts (one for local and one for production) for extended use, so that the production environment continues to work even when used locally.

## Licence

This project is licensed under the GNU License - see the LICENSE.md file for details.

## Author

Levin Joller
