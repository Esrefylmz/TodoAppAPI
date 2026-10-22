# TodoApp API

A RESTful Todo API built with ASP.NET Core and .NET 10, featuring JWT authentication, user-specific data management, and a layered architecture.

## Features

- Todo and Category CRUD operations
- User registration and login
- JWT authentication and authorization
- User-specific Todo and Category ownership
- Entity Framework Core with SQL Server
- Layered architecture with Service and Repository patterns
- Generic BaseRepository implementation
- AutoMapper and DTOs
- FluentValidation
- Global exception handling with ProblemDetails
- Filtering, searching, sorting, and pagination
- Swagger / OpenAPI documentation
- Docker and Docker Compose support
- Automated database migrations with Docker Compose

## Technologies

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022
- AutoMapper
- FluentValidation
- JWT Bearer Authentication
- Swagger / Swashbuckle
- Docker and Docker Compose

## Architecture

The solution is organized into five projects:

```text
TodoAppAPI
├── Controllers
├── Authentication & Authorization
├── Exception Handling
└── Dependency Injection Configuration

TodoAppApplication
├── Services
├── Service Interfaces
├── Validators
├── Mapping Profiles
└── Abstractions

TodoAppInfrastructure
├── Repositories
├── BaseRepository<T>
├── DbContext
└── EF Core Migrations

TodoAppCore
├── Entities
├── Repository Interfaces
└── Query Models

TodoAppDTOs
├── Todo DTOs
├── Category DTOs
├── Authentication DTOs
└── Common DTOs
```

### Request Flow

```text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
Repository Interface
     ↓
Repository Implementation
     ↓
Entity Framework Core
     ↓
SQL Server
```

The Service layer handles business logic, user ownership, filtering, sorting, and pagination.

Repositories provide data access and expose `IQueryable<T>` for list queries, allowing services to compose queries before execution.

Repository implementations inherit shared functionality from `BaseRepository<T>`.

Dependency Injection is used to connect interfaces with their implementations.

## Authentication

Authentication is handled using JWT Bearer tokens.

**Public endpoints:**

```http
POST /api/Auth/register
POST /api/Auth/login
```

Login returns a JWT token.

Protected endpoints require:

```text
Authorization: Bearer <token>
```

Passwords are securely hashed before being stored in the database.

Users can only access and modify their own Todos and Categories.

## Querying

Todo and Category endpoints support filtering, searching, sorting, and pagination.

Example:

```http
GET /api/Todo?isCompleted=false&sortBy=CreatedAt&sortDirection=Desc&pageNumber=1&pageSize=10
```

Pagination responses include:

- `items`
- `pageNumber`
- `pageSize`
- `totalCount`
- `totalPages`

Filtering and pagination are handled in the Service layer using `IQueryable<T>` queries provided by repositories.

## Docker Setup (Recommended)

Docker Compose runs the API and SQL Server in separate containers and automatically applies EF Core database migrations.

### Prerequisites

- Docker Desktop with Linux containers enabled

**The .NET SDK, EF Core CLI, and a local SQL Server installation are not required for Docker setup.**

### 1. Clone the repository

```powershell
git clone https://github.com/Esrefylmz/TodoAppAPI.git
cd TodoAppAPI
```

### 2. Configure environment variables

Create a `.env` file from the provided example:

```powershell
Copy-Item .env.example .env
```

Update `.env` with your own values:

```dotenv
MSSQL_SA_PASSWORD=your-strong-sql-server-password
JWT_KEY=your-long-random-jwt-signing-key
```

Use a strong SQL Server password that satisfies SQL Server's password complexity requirements.

For JWT signing, generate a random key using PowerShell:

```powershell
[Convert]::ToBase64String(
    [System.Security.Cryptography.RandomNumberGenerator]::GetBytes(48)
)
```

Copy the generated value into `JWT_KEY`.

**Never commit `.env` or real credentials to GitHub.**

### 3. Build and start the containers

```powershell
docker compose up -d --build
```

Docker Compose will:

1. Start the SQL Server container.
2. Wait for SQL Server to become healthy.
3. Run EF Core migrations using the migration container.
4. Start the ASP.NET Core API after migrations complete successfully.

No manual migration commands are required.

### 4. Verify containers

```powershell
docker compose ps -a
```

Expected container states:

| Service | Expected Status |
|---|---|
| sqlserver | Running (healthy) |
| migrate | Exited (0) |
| api | Running |

The migration container exits after successfully applying the migrations. This is expected behavior.

### 5. Access the API

Swagger UI:

http://localhost:5059/swagger/index.html

The SQL Server instance is accessible from the host at:

```text
127.0.0.1,14330
```

SQL Server Authentication uses the `sa` account and the password configured in `.env`.

### Docker Commands

Start the application:

```powershell
docker compose up -d
```

Rebuild and start:

```powershell
docker compose up -d --build
```

View container status:

```powershell
docker compose ps -a
```

View API logs:

```powershell
docker compose logs --tail=50 api
```

View migration logs:

```powershell
docker compose logs migrate
```

Stop containers:

```powershell
docker compose stop
```

Start stopped containers:

```powershell
docker compose start
```

Stop and remove containers while preserving database data:

```powershell
docker compose down
```

**Warning:** Running `docker compose down -v` also removes the database volume and permanently deletes the data stored in it.

## Local Development Setup (Without Docker)

The API can also be run directly using the .NET SDK and SQL Server LocalDB on Windows.

### Prerequisites

- .NET 10 SDK
- SQL Server LocalDB
- EF Core CLI

### 1. Restore dependencies

```powershell
dotnet restore
```

### 2. Install EF Core CLI

If `dotnet-ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.13
```

### 3. Configure JWT signing key

```powershell
dotnet user-secrets set "Jwt:Key" "your-long-random-secret-key" --project ./TodoAppAPI/TodoAppAPI.csproj
```

For local development, the JWT signing key is stored using .NET User Secrets rather than source-controlled configuration files.

### 4. Configure database

Ensure the `localdb` connection string in `TodoAppAPI/appsettings.json` points to your local SQL Server LocalDB instance.

### 5. Apply database migrations

```powershell
dotnet ef database update --project ./TodoAppInfrastructure/TodoAppInfrastructure.csproj --startup-project ./TodoAppAPI/TodoAppAPI.csproj
```

### 6. Run the API

```powershell
dotnet run --project ./TodoAppAPI/TodoAppAPI.csproj
```

Open the Swagger URL associated with the address printed in the terminal.

If the Docker API is running on the same port, stop it before starting the local application.

## Database

The application uses SQL Server with Entity Framework Core migrations.

Main entities:

- **User:** Authentication and ownership information
- **Todo:** Tasks belonging to users, optionally associated with a Category
- **Category:** User-specific task categories

Database relationships are configured using EF Core.

Deleting a Category sets the `CategoryId` of associated Todos to `null`, preserving the Todos.

The database also enforces a unique constraint on usernames.

## Configuration and Security

- Passwords are stored as hashes.
- JWT signing keys are not stored in source control.
- Protected endpoints require valid JWT tokens.
- Todo and Category ownership checks prevent cross-user access.
- FluentValidation validates incoming data.
- Global exception handling returns structured ProblemDetails responses.
- Docker credentials are configured through environment variables.