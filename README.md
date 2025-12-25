# RewardFlow - Employee Reward Management System

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat&logo=docker)](https://www.docker.com/)

Employee reward management system for Fayoum University. Automates reward calculations based on teaching sessions, type of the reward. and generates Ministry of Finance documentation.

## Overview

This is a REST API backend for managing employee rewards at educational institutions. The system handles employee data, calculates rewards based on teaching sessions, and prepares documentation for financial submission.

**Status**: Pre-production. Requires extensive testing before deployment due to handling sensitive financial data and regulatory requirements.

## Tech Stack

- .NET 9.0 / ASP.NET Core Web API
- SQL Server 2022
- Entity Framework Core 9.0
- JWT Authentication
- FluentValidation
- Docker

## Features

**Employee Management**
- CRUD operations with user-scoped access
- Fuzzy search using N-gram tokenization (handles Arabic names)
- Bulk import with validation
- AES encryption for sensitive fields (National ID, account numbers)

**Reward Calculations**
- Session-based reward computation
- Configurable business rules
- Employee-subject assignment tracking
- Automatic total recalculation

**Authentication**
- JWT with refresh tokens
- Role-based authorization (Admin/User)
- Plan-based access control

## Architecture

Uses Vertical Slices Architecture - features are organized by business capability rather than technical layers. Each slice contains its own endpoints, business logic, and data access.

**Domain Structure**:
```
Employees/    - Employee CRUD, search, bulk operations
Rewards/      - Reward calculations and management  
User/         - Authentication and authorization
Common/       - Shared services (encryption, tokenization, etc.)
```

**Database**: Single SQL Server database with multiple DbContexts for domain separation. Each domain has its own DbContext but shares the same database schema.

**Search**: Employee search uses N-gram tokenization (2-gram and 3-gram) with xxHash for performance. Tokens are pre-computed and stored for fast fuzzy matching.

## Setup

### Quick Start (Docker)

Run the entire application with one command:

```bash
docker-compose up -d
```

This starts:
- SQL Server at `localhost:1434`
- API at `http://localhost:5000` (redirects to API documentation)

Default credentials: `sa` / `Test123!@#`

### Local Development

```bash
cd RewardFlow.API
dotnet restore

# Run migrations
dotnet ef database update --context UserDbContext
dotnet ef database update --context EmployeeDbContext
dotnet ef database update --context RewardDbContext

dotnet run
```

API: `https://localhost:7071`  
Docs: `https://localhost:7071/scalar/v1`

### Configuration

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1434;Database=RewardFlow;User=sa;Password=Test123!@#;TrustServerCertificate=true"
  },
  "JWT": {
    "Token": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "EncryptionKey": "32-character-key-for-aes-256"
}
```

## Project Structure

```
RewardFlow.API/
├── Employees/
│   ├── CreateEmployee/
│   ├── UpdateEmployee/
│   ├── SearchEmployeesByName/
│   ├── BulkInsertEmployees/
│   ├── Data/              # DbContext, entities
│   └── Interceptors/      # Encryption interceptors
├── Rewards/
│   └── SessionsReward/
│       ├── EndPoints/
│       ├── Common/        # Calculation logic
│       └── Interface/
├── User/
│   ├── AuthService/
│   └── Data/
├── Common/
│   ├── Tokenization/      # N-gram tokenizer
│   ├── Encryption/        # AES service
│   ├── EmployeeLookup/    # Cross-domain access
│   └── BusinessRuleEngine/
└── Migrations/

RewardFlow.IntegrationTests/
RewardFlow.UnitTest/
```

## Key Implementation Details

**Factory Pattern**: `SessionRewardFactory` handles complex reward object creation and lifecycle management.

**Multiple DbContexts**: `EmployeeDbContext`, `RewardDbContext`, `UserDbContext` provide domain separation while sharing a database.

**EF Core Interceptors**: `EncryptionSaveChangesInterceptor` automatically encrypts/decrypts sensitive fields during save/load operations.

**Strategy Pattern**: `ISessionRewardCalculator` allows different reward calculation strategies.

**Service Layer**: Shared services like `EmployeeTokenService` and `EmployeeLookupService` enable cross-domain communication.

**Result Pattern**: Uses `FluentResults` for explicit success/failure handling without exceptions.

**Business Rules**: `IBusinessRule` interface with `BusinessRuleValidator` for domain validation logic.

## Testing

```bash
# Integration tests
cd RewardFlow.IntegrationTests
dotnet test

# Unit tests
cd RewardFlow.UnitTest
dotnet test
```

## Known Limitations

- PDF/Excel export not implemented yet
- No caching layer
- No rate limiting
- Basic audit logging
- Business rules are code-based (not externally configurable)

## Documentation

See `RewardFlow.API/Doc/` for:
- Architecture Decision Log
- N-gram search implementation details
- Project context and analysis

## License

MIT
