Technologie

- ASP NET Web API
- Angular
- PostgreSQL
- GitLab

System rezerwacji biletów do kina.

# Requirements
Ensure the following tools are installed on your system:

### Backend
- [.NET SDK 9.0+](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL 17+](https://www.postgresql.org/download/)

### Frontend
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
  ```bash
  npm install -g @angular/cli
  ```

# Getting started
## 1. Clone the repository
```bash
git clone https://github.com/sieradzki/cinema-dotnet.git
cd cinema
```

## 2. Create PostgreSQL database
- Name: cinema
- Username: postgres
- Password: admin

Or set your own and configure the connection string via .NET User Secrets (recommended) or environment variables.

### Local secrets (Development)
From the `api` folder, initialize and set user secrets once:

```bash
cd api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=cinema;Username=postgres;Password=YOUR_SECURE_PASSWORD"
dotnet user-secrets set "JWT:SigningKey" "YOUR_LONG_RANDOM_KEY"
# optional overrides
dotnet user-secrets set "JWT:Issuer" "http://localhost:5013"
dotnet user-secrets set "JWT:Audience" "http://localhost:4200"
```

### Production settings
Provide settings through environment variables (double underscore maps to nested keys):

```bash
export ConnectionStrings__DefaultConnection="Host=db;Database=cinema;Username=postgres;Password=***"
export JWT__SigningKey="***super-long-random***"
export JWT__Issuer="https://api.example.com"
export JWT__Audience="https://app.example.com"
```

## 3. Restore and Run the API
```bash
cd api
dotnet restore
dotnet ef database update  # Apply migrations
dotnet run # should be running on http://localhost:5013/
```

## 4. Frontend Setup

### Install dependencies
```bash
cd client
npm install
```

### Run angular server
```bash
ng s
```
By default, the app runs at: http://localhost:4200

Ensure that API calls from Angular point to the backend URL (https://localhost:5013). This may be configured in a file [client\src\app\environments\environment.ts](client\src\app\environments\environment.ts).

## Security and secrets
- No secrets are stored in the repository. Use .NET User Secrets for development and environment variables for production.
- To scan for accidental leaks locally (optional):
  - Install and run gitleaks or trufflehog.
  - Or quick check:
    ```bash
    git grep -n -I -E '(Password=|SigningKey|SECRET|ApiKey|client_secret)'
    ```
  - If something was ever committed, rotate it immediately. Optionally purge history with `git filter-repo`.