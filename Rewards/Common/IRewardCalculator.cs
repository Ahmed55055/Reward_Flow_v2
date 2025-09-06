namespace Reward_Flow_v2.Rewards.Common;

public interface IRewardCalculator
{
    int CalculateSessions(int numberOfStudents);
    float CalculateTotal(int numOfSessions, float salary, float percentage);
}