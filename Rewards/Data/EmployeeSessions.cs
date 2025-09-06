namespace Reward_Flow_v2.Rewards.Data;

public class EmployeeSessions
{
    public int Id { get; set; }
    public int SemesterSubjectId { get; set; }
    public int NumOfSessions { get; set; }
    public int EmpId { get; set; }
    public int SessionRewardId { get; set; }
    
    public virtual SubjectSemester SemesterSubject { get; set; } = null!;
    public virtual Employees.Data.Employee Employee { get; set; } = null!;
    public virtual SessionRewardEntity SessionReward { get; set; } = null!;
}