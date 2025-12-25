namespace Reward_Flow_v2.Employees.Data;

public class Department
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = null!;
    public int? FacultyId { get; set; }
    public bool IsDefault { get; set; }
    
    public virtual Faculty? Faculty { get; set; }
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();


}