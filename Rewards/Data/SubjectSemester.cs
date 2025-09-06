namespace Reward_Flow_v2.Rewards.Data;

public class SubjectSemester
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public byte SemesterNumber { get; set; }
    public int NumberOfStudents { get; set; }
    
    public virtual Subject Subject { get; set; } = null!;
    public virtual ICollection<EmployeeSessions> SessionsRewards { get; set; } = new List<EmployeeSessions>();
}