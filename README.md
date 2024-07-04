### Dokumentacja dla projektu MailBank_Backend

## Wprowadzenie

MailBank_Backend to usługa backendowa dla systemu bankowego, który umożliwia realizację transakcji za pomocą adresu e-mail zamiast numeru telefonu, podobnie jak system BLIK. Projekt jest napisany w języku C# i korzysta z platformy ASP.NET Core.

## Instalacja

### Wymagania systemowe

- .NET SDK
- Git

### Instrukcje instalacji

1. Sklonuj repozytorium:
   ```sh
   git clone https://github.com/UFEQ1337/MailBank_Backend.git
   cd MailBank_Backend
   ```

2. Zainstaluj zależności:
   ```sh
   dotnet restore
   ```

## Konfiguracja

### Pliki konfiguracyjne

#### appsettings.json

Plik `appsettings.json` zawiera podstawowe ustawienia konfiguracyjne dla aplikacji, takie jak łańcuchy połączeń do bazy danych.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourConnectionStringHere"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

#### appsettings.Development.json

Plik `appsettings.Development.json` zawiera ustawienia konfiguracyjne specyficzne dla środowiska deweloperskiego.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourDevelopmentConnectionStringHere"
  }
}
```

### Konfiguracja połączenia z bazą danych

Ustaw poprawne łańcuchy połączeń do bazy danych w plikach `appsettings.json` oraz `appsettings.Development.json`.

## Uruchamianie Aplikacji

Aby uruchomić aplikację lokalnie, użyj poniższej komendy w terminalu:

```sh
dotnet run --project Backend/Backend.csproj
```

## Struktura Projektu

### Controllers

Kontrolery MVC obsługują żądania HTTP i zarządzają logiką odpowiedzi. Znajdują się w katalogu `Backend/Controllers`.

### Models

Modele danych reprezentują strukturę danych używaną przez aplikację. Znajdują się w katalogu `Backend/Models`.

### Services

Serwisy aplikacyjne zawierają logikę biznesową aplikacji. Znajdują się w katalogu `Backend/Services`.

### Migrations

Migracje bazy danych umożliwiają zarządzanie zmianami w schemacie bazy danych. Znajdują się w katalogu `Backend/Migrations`.

### Program.cs

Plik `Program.cs` zawiera punkt wejścia aplikacji i konfiguruje uruchomienie serwera.

### MyDbContext.cs

Plik `MyDbContext.cs` konfiguruje kontekst bazy danych i definiuje DbSety dla modeli danych.



