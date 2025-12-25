namespace Reward_Flow_v2.Employees.Data;

public class Faculty
{
    public int FacultyId { get; set; }
    public string Name { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDefault { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

}