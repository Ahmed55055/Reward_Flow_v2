using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using RewardFlow.IntegrationTests.Infrastructure;
using Reward_Flow_v2.Employees.Data;
using Xunit;

namespace RewardFlow.IntegrationTests.Employees;

public class CreateEmployeeTests : BaseIntegrationTest
{
    public CreateEmployeeTests(TestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task CreateEmployee_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var randomId = Guid.NewGuid().ToString()[..8];
        var request = new
        {
            Name = "John Doe",
            NationalNumber = $"123456789{randomId}",
            AccountNumber = $"ACC{randomId}",
            Salary = 5000.0f,
            FacultyId = 1,
            DepartmentId = 1,
            JobTitle = (byte)1,
            Status = (byte)1
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Employees", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var employee = await response.Content.ReadFromJsonAsync<Employee>();
        employee.Should().NotBeNull();
        employee!.Name.Should().Be("John Doe");
        employee.NationalNumber.Should().Be($"123456789{randomId}");
    }

    [Fact]
    public async Task CreateEmployee_WithDuplicateNationalNumber_ShouldReturnBadRequest()
    {
        // Arrange
        var randomId = Guid.NewGuid().ToString()[..8];
        var duplicateNationalNumber = $"123456789{randomId}";
        
        await CreateTestEmployeeAsync("Existing Employee", duplicateNationalNumber);
        
        var request = new
        {
            Name = "John Doe",
            NationalNumber = duplicateNationalNumber, // Duplicate
            AccountNumber = $"ACC{Guid.NewGuid().ToString()[..8]}",
            Salary = 5000.0f,
            FacultyId = 1,
            DepartmentId = 1,
            JobTitle = (byte)1,
            Status = (byte)1
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Employees", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}