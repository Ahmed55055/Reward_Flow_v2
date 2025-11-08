namespace Reward_Flow_v2.Rewards;

internal static class RewardApiPath
{
    public const string Tag = "Rewards";
    private const string RewardRootApi = $"{ApiPath.Route}/{Tag}";

    public const string SessionsReward = $"{RewardRootApi}/sessions";
    public const string CreateSessionsReward = $"{SessionsReward}";
    public const string GetAllSessionsRewards = $"{SessionsReward}";
    public const string GetSessionsRewardById = $"{SessionsReward}/{{id}}";
    public const string UpdateSessionsReward = $"{SessionsReward}/{{id}}";
    public const string DeleteSessionsReward = $"{SessionsReward}/{{id}}";
    public const string GetSessionsRewardsByRewardId = $"{SessionsReward}/reward/{{rewardId}}";
    public const string AddEmployeeSessions = $"{SessionsReward}/{{id}}/employees";
    public const string AddMultipleEmployeeSessions = $"{SessionsReward}/{{id}}/employees/batch";
}