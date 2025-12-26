# RewardFlow Integration Tests

This project contains comprehensive integration tests for the RewardFlow API, specifically focusing on Employee endpoints with containerized database testing.

## Overview

The integration tests are designed to:
1. **Use containerized SQL Server database** - Each test run uses a fresh SQL Server container
2. **Apply EF Core migrations automatically** - Database schema is created and migrated before tests
3. **Test all Employee endpoints** - Comprehensive coverage of CRUD operations
4. **Isolate test data** - Each test seeds its own data and cleans up afterward
5. **Full lifecycle testing** - Complete user workflows from creation to deletion

## Architecture

### Test Infrastructure
- **TestWebApplicationFactory**: Sets up the test server with containerized database
- **BaseIntegrationTest**: Base class providing database seeding and cleanup
- **TestAuthenticationHandler**: Mock authentication for testing

### Database Container
- Uses **Testcontainers** library for SQL Server 2022
- Automatic container lifecycle management
- EF Core migrations applied automatically
- Fresh database for each test run

### Test Categories

#### Individual Endpoint Tests
- `CreateEmployeeTests` - Employee creation scenarios
- `GetEmployeeTests` - Employee retrieval operations
- `UpdateEmployeeTests` - Employee modification scenarios
- `DeleteEmployeeTests` - Employee deletion operations
- `EmployeeSearchAndBulkTests` - Search and bulk operations

#### Full Lifecycle Tests
- `EmployeeFullLifecycleTests` - Complete employee workflow testing

## Running Tests

### Prerequisites
- Docker installed and running
- .NET 9.0 SDK
- SQL Server container support

### Quick Start
```bash
# Make the script executable
chmod +x run-tests.sh

# Run all integration tests
./run-tests.sh
```

### Manual Execution
```bash
# Build the project
dotnet build

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Docker Compose (Alternative)
```bash
# Start test database manually
docker-compose -f docker-compose.test.yml up -d

# Run tests
dotnet test

# Clean up
docker-compose -f docker-compose.test.yml down -v
```

## Test Data Management

### Seeding Strategy
- **Required data** (Faculty, Department, JobTitle, EmployeeStatus) is seeded automatically
- **Test-specific data** is created per test using `CreateTestEmployeeAsync()`
- **Database reset** happens between tests to ensure isolation

### Data Isolation
Each test class inherits from `BaseIntegrationTest` which:
- Seeds required reference data before each test
- Cleans up test data after each test
- Ensures no test interference

## Employee Endpoints Tested

| Endpoint | Method | Test Coverage |
|----------|--------|---------------|
| `/api/Employees` | POST | Create employee with validation |
| `/api/Employees` | GET | Get all employees |
| `/api/Employees/{id}` | GET | Get employee by ID |
| `/api/Employees/{id}` | PUT | Update employee |
| `/api/Employees/{id}` | DELETE | Delete employee |
| `/api/Employees/national/{number}` | GET | Get by national number |
| `/api/Employees/name/{name}` | GET | Get by name |
| `/api/Employees/search?name={query}` | GET | Search employees |
| `/api/Employees/BulkInsert` | POST | Bulk insert employees |

## Test Scenarios

### Positive Test Cases
- ✅ Create employee with valid data
- ✅ Retrieve employee by various identifiers
- ✅ Update employee information
- ✅ Delete employee
- ✅ Search employees by name
- ✅ Bulk insert multiple employees
- ✅ Full employee lifecycle workflow

### Negative Test Cases
- ❌ Create employee with duplicate national number
- ❌ Get non-existent employee
- ❌ Update non-existent employee
- ❌ Delete non-existent employee
- ❌ Bulk insert with duplicate data

## Configuration

### Database Connection
- **Container**: SQL Server 2022 Latest
- **Port**: 1434 (mapped from container's 1433)
- **Credentials**: sa/Test123!@#
- **Connection Timeout**: 120 seconds

### Test Authentication
- Uses mock authentication handler
- Test user ID: 1
- No actual JWT tokens required

## Troubleshooting

### Common Issues

**Docker not running**
```
Error: Docker is not running
```
Solution: Start Docker Desktop or Docker daemon

**Port conflicts**
```
Port 1434 already in use
```
Solution: Stop existing containers or change port in docker-compose.test.yml

**Migration failures**
```
Database migration failed
```
Solution: Ensure EF Core tools are installed and migrations exist

**Test timeouts**
```
Test execution timeout
```
Solution: Increase timeout in TestWebApplicationFactory or check container health

### Debugging

1. **Check container status**:
   ```bash
   docker ps
   docker logs <container-id>
   ```

2. **Verify database connectivity**:
   ```bash
   docker exec -it <container-id> /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P Test123!@# -C
   ```

3. **Run individual test classes**:
   ```bash
   dotnet test --filter "FullyQualifiedName~CreateEmployeeTests"
   ```

## Contributing

When adding new tests:
1. Inherit from `BaseIntegrationTest`
2. Use `CreateTestEmployeeAsync()` for test data
3. Follow the naming convention: `{Operation}_{Scenario}_{ExpectedResult}`
4. Clean up any additional test data if needed
5. Add both positive and negative test cases

## Performance Considerations

- Container startup adds ~10-15 seconds to test execution
- Database migrations add ~5-10 seconds
- Consider running tests in parallel for large test suites
- Use `IClassFixture<TestWebApplicationFactory>` to share container across test class