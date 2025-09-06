namespace Reward_Flow_v2.Rewards.Data.Database;

public class EmployeeRewardEntity
{
    public int Id {  get; set; }
    public int EmployeeId { get; set; }
    public int RewardId { get; set; }
    public decimal Total { get; set; }

}
