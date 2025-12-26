namespace Reward_Flow_v2.Common.EmployeeLookup;

public record EmployeeDto
{
    public int EmployeeId { get; init; }
    public string Name { get; init; }
    public float? Salary { get; init; }
    public string NationalNumber { get; init; }
}
