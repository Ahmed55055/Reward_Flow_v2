namespace Reward_Flow_v2.Rewards.SessionsReward.Common;

public interface ISessionRewardRules
{
    int MaxSessionsNumber { get; set; }
    bool MaxSessionNumberRuleEnabled {  get; set; }

    bool IsWithInMaximumNumberOfSession(int sessionNumber);
}