namespace Reward_Flow_v2.Rewards.Data;

public class EmployeeReward
{
    public int Id { get; set; }
    public int RewardId { get; set; }
    public int EmpId { get; set; }
    public float Total { get; set; }
    
    public virtual RewardEntity Reward { get; set; } = null!;
    public virtual Employees.Data.Employee Employee { get; set; } = null!;
}