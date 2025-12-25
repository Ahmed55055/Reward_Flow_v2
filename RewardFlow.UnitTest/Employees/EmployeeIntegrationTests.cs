using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reward_Flow_v2.Employees.Data;
using Reward_Flow_v2.Employees.Data.Database;
using System.Net.Http.Json;
using FluentAssertions;
using System.Net;
using Reward_Flow_v2.Employees.CreateEmployee;
using Reward_Flow_v2.Employees.UpdateEmployee;
using Reward_Flow_v2.Employees.BulkInsertEmployees;
using Reward_Flow_v2;

namespace RewardFlow_UnitTest.Employees;

public class EmployeeIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly EmployeeDbContext _context;
    private readonly IServiceScope _scope;

    public EmployeeIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove all DbContext registrations
                var descriptors = services.Where(d => 
                    d.ServiceType == typeof(DbContextOptions<EmployeeDbContext>) ||
                    d.ServiceType == typeof(EmployeeDbContext)).ToList();
                
                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                // Add InMemory database
                services.AddDbContext<EmployeeDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            });
        });

        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
        SeedTestData();
    }

    [Fact]
    public async Task CreateEmployee_ValidRequest_ReturnsCreated()
    {
        var request = new CreateEmployee.Request("John Doe", "123456789", "ACC001", 5000f, null, null, 1, 1);

        var response = await _client.PostAsJsonAsync("/api/employees", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var employee = await response.Content.ReadFromJsonAsync<Employee>();
        employee.Should().NotBeNull();
        employee.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task CreateEmployee_InvalidRequest_ReturnsBadRequest()
    {
        var request = new CreateEmployee.Request("", "123456789", null, -100f, null, null, null, null);

        var response = await _client.PostAsJsonAsync("/api/employees", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsEmployees()
    {
        var response = await _client.GetAsync("/api/employees?limit=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
        employees.Should().NotBeNull();
        employees.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetEmployeeById_ExistingId_ReturnsEmployee()
    {
        var employee = _context.Employee.First();

        var response = await _client.GetAsync($"/api/employees/{employee.EmployeeId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Employee>();
        result.Should().NotBeNull();
        result.EmployeeId.Should().Be(employee.EmployeeId);
    }

    [Fact]
    public async Task GetEmployeeById_NonExistentId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/employees/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetEmployeeByName_ExistingName_ReturnsEmployee()
    {
        var employee = _context.Employee.First();

        var response = await _client.GetAsync($"/api/employees/name/{employee.Name}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Employee>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetEmployeeByNationalNumber_ExistingNumber_ReturnsEmployee()
    {
        var employee = _context.Employee.First();

        var response = await _client.GetAsync($"/api/employees/national/{employee.NationalNumber}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Employee>();
        result.Should().NotBeNull();
        result.NationalNumber.Should().Be(employee.NationalNumber);
    }

    [Fact]
    public async Task SearchEmployeesByName_ValidName_ReturnsEmployees()
    {
        var response = await _client.GetAsync("/api/employees/search?name=Test&limit=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
        employees.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateEmployee_ValidRequest_ReturnsNoContent()
    {
        var employee = _context.Employee.First();
        var request = new UpdateEmployee.Request("Updated Name", null, null, 6000f, null, null, null, null);

        var response = await _client.PatchAsJsonAsync($"/api/employees/{employee.EmployeeId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateEmployee_NonExistentId_ReturnsNotFound()
    {
        var request = new UpdateEmployee.Request("Updated Name", null, null, null, null, null, null, null);

        var response = await _client.PatchAsJsonAsync("/api/employees/999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteEmployee_ExistingId_ReturnsNoContent()
    {
        var employee = _context.Employee.First();

        var response = await _client.DeleteAsync($"/api/employees/{employee.EmployeeId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task BulkInsertEmployees_ValidRequest_ReturnsAccepted()
    {
        var employees = new List<BulkInsert.emp>
        {
            new() { Name = "Bulk Employee 1", NationalNumber = "111111111", Salary = 4000f },
            new() { Name = "Bulk Employee 2", NationalNumber = "222222222", Salary = 4500f }
        };
        var request = new BulkInsert.Request(employees);

        var response = await _client.PostAsJsonAsync("/api/employees/BulkInsert", request);

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var result = await response.Content.ReadFromJsonAsync<BulkInsert.Response>();
        result.Should().NotBeNull();
        result.Success.Should().BeGreaterThan(0);
    }

    private void SeedTestData()
    {
        if (!_context.Employee.Any())
        {
            _context.Employee.AddRange(
                new Employee
                {
                    Name = "Test Employee 1",
                    NationalNumber = "123456789",
                    AccountNumber = "ACC001",
                    Salary = 5000f,
                    CreatedBy = 1,
                    CreatedAt = DateTime.UtcNow,
                    Status = 1,
                    IsActive = true
                },
                new Employee
                {
                    Name = "Test Employee 2", 
                    NationalNumber = "987654321",
                    AccountNumber = "ACC002",
                    Salary = 5500f,
                    CreatedBy = 1,
                    CreatedAt = DateTime.UtcNow,
                    Status = 1,
                    IsActive = true
                }
            );
            _context.SaveChanges();
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
        _scope?.Dispose();
        _client?.Dispose();
    }
}