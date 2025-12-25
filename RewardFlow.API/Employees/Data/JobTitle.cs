namespace Reward_Flow_v2.Employees.Data;

public class JobTitle
{
    public byte JobTitleId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();


}