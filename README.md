# 📄 Notes API (C# .NET + Dapper + EF + SQL Server + Swagger)

## Overview

This project is a **RESTful API** built with **C# .NET 9**, using:

- **Dapper ORM** for lightweight and fast data access
- **Entity Framework Core** for database migrations
- **SQL Server** as the database
- Provides **User** and **Note** management endpoints
- **Swagger** for interactive API documentation/testing

The API supports **CRUD operations** for users and notes, authentication (login/register), and uses EF migrations for schema management.

## ⚙️ Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local or cloud)
- IDE: Visual Studio / VS Code
- Optional: Postman or HTTP client for testing

---

## 🛠️ Setup

### 1. Clone the repository

```bash
git clone git@github.com:vannsoklay/NoteAPI.git
cd NoteAPI
```

### 2. Configure Database

Update **appsettings.json**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotesDB;User Id=sa;Password=YourPassword;"
  }
}
```

### 3. Docker Database

```
# You can connect to the database using your backend .env settings:
# MySQL Configuration

MYSQL_ROOT_PASSWORD=helloworld
MYSQL_DATABASE=test
MYSQL_USER=user
MYSQL_PASSWORD=helloworld123
MYSQL_PORT=3306

```

```bash
# run container
docker-compose up -d
# show container 
docker ps
# stop container
docker-compose down
```

> Make sure SQL Server is running and accessible.

### 4. Apply EF Migrations

Generate the database schema using Entity Framework migrations:

```bash
# Add migration
dotnet ef migrations add InitialCreate -o src/Migrations/

# Update database
dotnet ef database update
```

---

## 🏷️ Swagger Integration

Swagger is included to **document and test APIs** interactively.

### 1. Install Swagger NuGet Packages

```bash
dotnet add package BCrypt.Net-Core
dotnet add package Dapper
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore.SwaggerGen
dotnet add package Swashbuckle.AspNetCore.SwaggerUI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 2. Run the API and Access Swagger

```bash
dotnet run
```

Open browser at:

```
http://localhost:5250/index.html
```

or  

```
http://localhost:5250/index.html
```

You will see **Swagger UI** for testing all endpoints.

---

## 🚀 Running the Project

```bash
dotnet restore
dotnet build
dotnet run
```

The API will start at:

```
http://localhost:5250
```

---

## 📡 API Endpoints

- Check on Swagger (http://localhost:5250/index.html)
---

## 🔐 Authentication

- Currently uses **header-based user ID** (`X-User-Id`) for identifying the user.
- No JWT or cookie-based auth implemented (can be added later).

---

## 📦 Dapper Example

```csharp
public async Task<User> GetUserByIdAsync(string id)
{
    using var connection = new SqlConnection(_connectionString);
    var query = "SELECT * FROM Users WHERE Id = @Id";
    return await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
}
```

---

## 🔗 References

- [Dapper Documentation](https://dapper-tutorial.net/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Swashbuckle/Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
