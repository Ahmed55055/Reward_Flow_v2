using Microsoft.Extensions.DependencyInjection;
using Reward_Flow_v2.Employees.Data;
using Reward_Flow_v2.Employees.Data.Database;
using Reward_Flow_v2.User.Data;
using Reward_Flow_v2.User.Data.Database;
using Xunit;

namespace RewardFlow.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected readonly TestWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "test-token");
    }

    public virtual async Task InitializeAsync()
    {
        await SeedRequiredDataAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    protected async Task SeedRequiredDataAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var employeeDbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
        var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

// Seed test user if not exists
        if (!userDbContext.User.Any(u => u.UUID == TestAuthenticationHandler.TestUserGuid))
        {
            var user = new User
            {
                Username = Guid.NewGuid().ToString(), 
                PasswordHash = "test-hash",
                Email = "test@example.com",
                RoleId = 3, // User role
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                PlanId = 1 // Free plan
            };

            userDbContext.User.Add(user);
            await userDbContext.SaveChangesAsync();
            TestAuthenticationHandler.TestUserGuid = user.UUID;
        }
        
        // Seed Faculty if not exists
        if (!employeeDbContext.Faculty.Any())
        {
            employeeDbContext.Faculty.AddRange(
                new Faculty { Name = "Engineering", CreatedBy = 1, CreatedAt = DateTime.UtcNow },
                new Faculty { Name = "Science", CreatedBy = 1, CreatedAt = DateTime.UtcNow }
            );
            await employeeDbContext.SaveChangesAsync();
        }


        // Seed Department if not exists
        if (!employeeDbContext.Department.Any())
        {
            employeeDbContext.Department.AddRange(
                new Department { Name = "Computer Science", FacultyId = 1 },
                new Department { Name = "Electrical Engineering", FacultyId = 1 }
            );
            await employeeDbContext.SaveChangesAsync();
        }
    }

    protected async Task<Employee> CreateTestEmployeeAsync(string name = "Test Employee",
        string nationalNumber = "12345678901")
    {
        using var scope = Factory.Services.CreateScope();
        var employeeDbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();

        var employee = new Employee
        {
            Name = name,
            NationalNumber = nationalNumber,
            AccountNumber = $"ACC{Random.Shared.Next(1000, 9999)}",
            Salary = 5000.0f,
            FacultyId = 1,
            DepartmentId = 1,
            JobTitle = 1,
            Status = 1,
            CreatedBy = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        employeeDbContext.Employee.Add(employee);
        await employeeDbContext.SaveChangesAsync();

        return employee;
    }
}