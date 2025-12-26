using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards;

public class EmployeeRewardUtility
{
    private readonly RewardDbContext _dbContext;
    public EmployeeRewardUtility(RewardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public EmployeeReward? FindEmployeeReward(int employeeId, int rewardId)
    {
        return _dbContext.EmployeeReward.FirstOrDefault(er => er.EmployeeId == employeeId && er.RewardId == rewardId);
    }

    public IEnumerable<EmployeeReward> GetEmployeeRewards(IEnumerable<int> employeesIds, int rewardId)
    {
        return _dbContext.EmployeeReward.Where(er => employeesIds.Contains(er.EmployeeId) && er.RewardId == rewardId).ToList();
    }

    public void SetAdd(EmployeeReward employeeReward)
    {
        _dbContext.EmployeeReward.Add(employeeReward);
    }

    public void SetAdd(IEnumerable<EmployeeReward> employeeRewards)
    {
        _dbContext.EmployeeReward.AddRange(employeeRewards);
    }

    public void SetUpdate(EmployeeReward employeeReward)
    {
        _dbContext.EmployeeReward.Update(employeeReward);
    }
    public void SetUpdate(IEnumerable<EmployeeReward> employeeRewards)
    {
        _dbContext.EmployeeReward.UpdateRange(employeeRewards);
    }
}
