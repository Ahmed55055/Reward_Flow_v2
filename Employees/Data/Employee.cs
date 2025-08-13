namespace Reward_Flow_v2.Employees.Data;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = null!;
    public string NationalNumber { get; set; } = null!;
    public string? AccountNumber { get; set; }
    public float? Salary { get; set; }
    public int? FacultyId { get; set; }
    public int? DepartmentId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte? JobTitle { get; set; }
    public bool IsActive { get; set; }
    public byte? Status { get; set; }

    public virtual Department? Department { get; set; }
    public virtual Faculty? Faculty { get; set; }
    public virtual JobTitle? JobTitleNavigation { get; set; }
    public virtual EmployeeStatus? StatusNavigation { get; set; }

}
