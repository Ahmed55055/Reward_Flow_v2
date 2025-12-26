using System.Net;
using FluentAssertions;
using RewardFlow.IntegrationTests.Infrastructure;
using Xunit;

namespace RewardFlow.IntegrationTests.Employees;

public class DeleteEmployeeTests : BaseIntegrationTest
{
    public DeleteEmployeeTests(TestWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task DeleteEmployee_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var testEmployee = await CreateTestEmployeeAsync("Employee To Delete", "12345678901");

        // Act
        var response = await Client.DeleteAsync($"/api/Employees/{testEmployee.EmployeeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify deletion
        var getResponse = await Client.GetAsync($"/api/Employees/{testEmployee.EmployeeId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteEmployee_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await Client.DeleteAsync("/api/Employees/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}