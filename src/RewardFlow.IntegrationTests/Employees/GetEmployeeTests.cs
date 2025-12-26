using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using RewardFlow.IntegrationTests.Infrastructure;
using Reward_Flow_v2.Employees.Data;
using Xunit;

namespace RewardFlow.IntegrationTests.Employees;

public class GetEmployeeTests : BaseIntegrationTest
{
    public GetEmployeeTests(TestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetEmployeeById_WithValidId_ShouldReturnEmployee()
    {
        // Arrange
        var testEmployee = await CreateTestEmployeeAsync("Test Employee", "12345678901");

        // Act
        var response = await Client.GetAsync($"/api/Employees/{testEmployee.EmployeeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employee = await response.Content.ReadFromJsonAsync<Employee>();
        employee.Should().NotBeNull();
        employee!.EmployeeId.Should().Be(testEmployee.EmployeeId);
        employee.Name.Should().Be("Test Employee");
    }

    [Fact]
    public async Task GetEmployeeById_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await Client.GetAsync("/api/Employees/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetEmployeeByNationalNumber_WithValidNumber_ShouldReturnEmployee()
    {
        // Arrange
        var testEmployee = await CreateTestEmployeeAsync("Test Employee", "12345678901");

        // Act
        var response = await Client.GetAsync($"/api/Employees/national/{testEmployee.NationalNumber}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employee = await response.Content.ReadFromJsonAsync<Employee>();
        employee.Should().NotBeNull();
        employee!.NationalNumber.Should().Be("12345678901");
    }

    [Fact]
    public async Task GetEmployeeByName_WithValidName_ShouldReturnEmployee()
    {
        // Arrange
        var testEmployee = await CreateTestEmployeeAsync("John Smith", "12345678901");

        // Act
        var response = await Client.GetAsync($"/api/Employees/name/{Uri.EscapeDataString("John Smith")}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employee = await response.Content.ReadFromJsonAsync<Employee>();
        employee.Should().NotBeNull();
        employee!.Name.Should().Be("John Smith");
    }

    [Fact]
    public async Task GetAllEmployees_ShouldReturnEmployeeList()
    {
        // Arrange
        await CreateTestEmployeeAsync("Employee 1", "11111111111");
        await CreateTestEmployeeAsync("Employee 2", "22222222222");

        // Act
        var response = await Client.GetAsync("/api/Employees");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
        employees.Should().NotBeNull();
        employees!.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}