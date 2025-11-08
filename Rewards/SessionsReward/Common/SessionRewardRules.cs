namespace Reward_Flow_v2.Rewards.SessionsReward.Common;

public class SessionRewardRules : ISessionRewardRules
{
    // TODO: Move this to database configuration in the future
    private const int MAX_SESSIONS_LIMIT = 250;

    public SessionRewardRules()
    {
        MaxSessionsNumber = MAX_SESSIONS_LIMIT;
        MaxSessionNumberRuleEnabled = true;
    }

    public int MaxSessionsNumber { get; set; }
    public bool MaxSessionNumberRuleEnabled { get; set; }

    public bool IsWithInMaximumNumberOfSession(int sessionNumber)
    {
        return !MaxSessionNumberRuleEnabled || sessionNumber <= MaxSessionsNumber;
    }
}
