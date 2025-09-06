using Reward_Flow_v2.Rewards.SessionsReward.CreateSessionsReward;
using Reward_Flow_v2.Rewards.SessionsReward.DeleteSessionsReward;
using Reward_Flow_v2.Rewards.SessionsReward.GetAllSessionsRewards;
using Reward_Flow_v2.Rewards.SessionsReward.GetSessionsRewardById;
using Reward_Flow_v2.Rewards.SessionsReward.GetSessionsRewardsByReward;
using Reward_Flow_v2.Rewards.SessionsReward.UpdateSessionsReward;

namespace Reward_Flow_v2.Rewards;

public static class RewardEndpoints
{
    public static void MapRewardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateSessionsReward();
        app.MapGetAllSessionsRewards();
        app.MapGetSessionsRewardById();
        app.MapUpdateSessionsReward();
        app.MapDeleteSessionsReward();
        app.MapGetSessionsRewardsByReward();
    }
}