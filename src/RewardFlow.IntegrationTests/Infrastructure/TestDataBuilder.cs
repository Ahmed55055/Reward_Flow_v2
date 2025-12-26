using Reward_Flow_v2.Employees.Data;

namespace RewardFlow.IntegrationTests.Infrastructure;

public static class TestDataBuilder
{
    public static Employee CreateTestEmployee(string name = "John Doe", string nationalNumber = "12345678901")
    {
        return new Employee
        {
            Name = name,
            NationalNumber = nationalNumber,
            AccountNumber = "ACC123456",
            Salary = 5000.0f,
            FacultyId = 1,
            DepartmentId = 1,
            JobTitle = 1,
            Status = 1,
            CreatedBy = 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public static List<Employee> CreateTestEmployees(int count)
    {
        var employees = new List<Employee>();
        for (int i = 1; i <= count; i++)
        {
            employees.Add(CreateTestEmployee($"Employee {i}", $"1234567890{i:D2}"));
        }
        return employees;
    }
}