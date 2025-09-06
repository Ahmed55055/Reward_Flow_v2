namespace Reward_Flow_v2.Rewards.Common;

public class SessionsRewardCalculator : IRewardCalculator
{
    public int CalculateSessions(int numberOfStudents)
    {
        if (numberOfStudents < 5)
            return (int)Math.Ceiling(numberOfStudents / 5.0);
        
        return (int)Math.Round(numberOfStudents / 5.0, MidpointRounding.AwayFromZero);
    }

    public float CalculateTotal(int numOfSessions, float salary, float percentage)
    {
        return numOfSessions * salary * percentage;
    }
}