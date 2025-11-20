using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward;

public interface IRewardDbContextFactory
{
    public RewardDbContext CreateDbContext();
}