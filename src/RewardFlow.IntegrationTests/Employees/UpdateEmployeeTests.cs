using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using RewardFlow.IntegrationTests.Infrastructure;
using Reward_Flow_v2.Employees.Data;
using Xunit;

namespace RewardFlow.IntegrationTests.Employees;

public class UpdateEmployeeTests : BaseIntegrationTest
{
    public UpdateEmployeeTests(TestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task UpdateEmployee_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var testEmployee = await CreateTestEmployeeAsync("Original Name", "12345678901");
        
        var updateRequest = new
        {
            Name = "Updated Name",
            NationalNumber = "12345678901",
            AccountNumber = "ACC654321",
            Salary = 6000.0f,
            FacultyId = 1,
            DepartmentId = 1,
            JobTitle = (byte)2,
            Status = (byte)1
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/Employees/{testEmployee.EmployeeId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Verify the update
        var getResponse = await Client.GetAsync($"/api/Employees/{testEmployee.EmployeeId}");
        var updatedEmployee = await getResponse.Content.ReadFromJsonAsync<Employee>();
        updatedEmployee!.Name.Should().Be("Updated Name");
        updatedEmployee.Salary.Should().Be(6000.0f);
    }

    [Fact]
    public async Task UpdateEmployee_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var updateRequest = new
        {
            Name = "Updated Name",
            NationalNumber = "12345678901",
            AccountNumber = "ACC654321",
            Salary = 6000.0f,
            FacultyId = 1,
            DepartmentId = 1,
            JobTitle = (byte)2,
            Status = (byte)1
        };

        // Act
        var response = await Client.PutAsJsonAsync("/api/Employees/99999", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}