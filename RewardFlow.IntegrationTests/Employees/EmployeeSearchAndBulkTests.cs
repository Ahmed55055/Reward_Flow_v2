using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using RewardFlow.IntegrationTests.Infrastructure;
using Reward_Flow_v2.Employees.Data;
using Xunit;

namespace RewardFlow.IntegrationTests.Employees;

public class EmployeeSearchAndBulkTests : BaseIntegrationTest
{
    public EmployeeSearchAndBulkTests(TestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task SearchEmployeesByName_WithMatchingName_ShouldReturnResults()
    {
        // Arrange
        await CreateTestEmployeeAsync("John Smith", "11111111111");
        await CreateTestEmployeeAsync("John Doe", "22222222222");
        await CreateTestEmployeeAsync("Jane Wilson", "33333333333");

        // Act
        var response = await Client.GetAsync("/api/Employees/search?name=John");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
        employees.Should().NotBeNull();
        employees!.Count.Should().BeGreaterThanOrEqualTo(2);
        employees.Should().OnlyContain(e => e.Name.Contains("John"));
    }

    [Fact]
    public async Task SearchEmployeesByName_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        await CreateTestEmployeeAsync("John Smith", "11111111111");

        // Act
        var response = await Client.GetAsync("/api/Employees/search?name=NonExistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
        employees.Should().NotBeNull();
        employees!.Should().BeEmpty();
    }

    [Fact]
    public async Task BulkInsertEmployees_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var bulkEmployees = new[]
        {
            new { Name = "Alice Johnson", NationalNumber = "55566677788", AccountNumber = "ACC999", Salary = 4800.0f, FacultyId = 1, DepartmentId = 1, JobTitle = (byte)1, Status = (byte)1 },
            new { Name = "Charlie Brown", NationalNumber = "99988877766", AccountNumber = "ACC888", Salary = 5200.0f, FacultyId = 1, DepartmentId = 1, JobTitle = (byte)1, Status = (byte)1 }
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Employees/BulkInsert", bulkEmployees);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Verify employees were created
        var getAllResponse = await Client.GetAsync("/api/Employees");
        var allEmployees = await getAllResponse.Content.ReadFromJsonAsync<List<Employee>>();
        allEmployees!.Should().Contain(e => e.Name == "Alice Johnson");
        allEmployees.Should().Contain(e => e.Name == "Charlie Brown");
    }

    [Fact]
    public async Task BulkInsertEmployees_WithDuplicateNationalNumbers_ShouldReturnBadRequest()
    {
        // Arrange
        await CreateTestEmployeeAsync("Existing Employee", "55566677788");
        
        var bulkEmployees = new[]
        {
            new { Name = "Alice Johnson", NationalNumber = "55566677788", AccountNumber = "ACC999", Salary = 4800.0f, FacultyId = 1, DepartmentId = 1, JobTitle = (byte)1, Status = (byte)1 }
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Employees/BulkInsert", bulkEmployees);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}