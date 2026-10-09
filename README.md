# TodoApp API

A RESTful Todo API built with ASP.NET Core and .NET 10.

## Features

- Todo and Category CRUD
- User registration and login
- JWT authentication and authorization
- User-specific Todo and Category ownership
- Entity Framework Core with SQL Server
- Repository and Service patterns
- AutoMapper and DTOs
- FluentValidation
- Global exception handling
- Filtering, sorting and pagination
- Swagger / OpenAPI

## Technologies

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- JWT Bearer Authentication
- Swagger / Swashbuckle

## Architecture

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
SQL Server
```

## Authentication

```text
POST /api/Auth/register
POST /api/Auth/login
```

Login returns a JWT token.

Protected endpoints require:

```text
Authorization: Bearer <token>
```

## Querying

Todo and Category endpoints support filtering/search, sorting and pagination.

Example:

```http
GET /api/Todo?isCompleted=false&sortBy=CreatedAt&sortDirection=Desc&pageNumber=1&pageSize=10
```

## Setup

Clone the repository and restore the dependencies:

```bash
dotnet restore
```

If `dotnet ef` is not installed, install it globally:

```bash
dotnet tool install --global dotnet-ef
```

Configure the JWT secret:

```bash
dotnet user-secrets set "Jwt:Key" "your-long-secret-key" --project ./TodoAppAPI/TodoAppAPI.csproj
```

Apply the database migrations:

```bash
dotnet ef database update --project ./TodoAppInfrastructure/TodoAppInfrastructure.csproj --startup-project ./TodoAppAPI/TodoAppAPI.csproj
```

Run the API:

```bash
dotnet run --project ./TodoAppAPI/TodoAppAPI.csproj
```

Then open Swagger in your browser to test the API.