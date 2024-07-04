# MailBank Backend

## Opis projektu

MailBank Backend to aplikacja serwerowa odpowiedzialna za obsługę i zarządzanie kontami e-mail oraz ich integrację z bankowymi systemami. Projekt jest częścią większego systemu MailBank, który ma na celu usprawnienie zarządzania komunikacją e-mailową w środowisku bankowym.

## Wymagania systemowe

- .NET Core SDK w wersji 3.1 lub nowszej
- SQL Server 2019

## Instalacja

1. Sklonuj repozytorium:

    ```bash
    git clone https://github.com/UFEQ1337/MailBank_Backend.git
    cd MailBank_Backend
    ```

2. Otwórz projekt w Visual Studio:

    - Wybierz `File -> Open -> Project/Solution`
    - Wskaż plik `Backend.sln` znajdujący się w katalogu głównym repozytorium

3. Przygotuj bazę danych SQL Server:

    - Utwórz nową bazę danych w SQL Server
    - Zaktualizuj connection string w pliku `appsettings.json` w projekcie `Backend`:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=<adres_serwera>;Database=<nazwa_bazy_danych>;User Id=<użytkownik>;Password=<hasło>;"
    }
    ```

4. Uruchom migracje, aby utworzyć niezbędne tabele w bazie danych:

    ```bash
    dotnet ef database update
    ```

5. Uruchom aplikację:

    - Kliknij przycisk `IIS Express` w Visual Studio lub użyj komendy:

    ```bash
    dotnet run --project Backend
    ```

## Użycie

Aplikacja będzie dostępna na domyślnym porcie 5000. Możesz przetestować działanie API za pomocą narzędzi takich jak Postman, wysyłając żądania do `http://localhost:5000/api`.

## Konfiguracja

Plik `appsettings.json` powinien zawierać następujące ustawienia:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<adres_serwera>;Database=<nazwa_bazy_danych>;User Id=<użytkownik>;Password=<hasło>;"
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
