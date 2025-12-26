using Reward_Flow_v2.Rewards.Common;

namespace Reward_Flow_v2.Rewards.SessionsReward.Interface;

public interface ISessionRewardCalculator : IRewardCalculator
{
    int CalculateSessions(int numberOfStudents);

}