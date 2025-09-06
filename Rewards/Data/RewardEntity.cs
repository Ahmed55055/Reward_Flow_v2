namespace Reward_Flow_v2.Rewards.Data;

public class RewardEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public float Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public int CreatedBy { get; set; }
    public string? Code { get; set; }
    public int RewardType { get; set; }
    public int NumberOfEmployees { get; set; }

    public virtual ICollection<EmployeeSessions> SessionsRewards { get; set; } = new List<EmployeeSessions>();
    public virtual ICollection<EmployeeReward> EmployeeRewards { get; set; } = new List<EmployeeReward>();
}