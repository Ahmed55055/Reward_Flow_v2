namespace Reward_Flow_v2.Rewards.Common;

public interface IRewardCalculator
{
    float CalculateTotal(int numOfSessions, float salary, float percentage);
}