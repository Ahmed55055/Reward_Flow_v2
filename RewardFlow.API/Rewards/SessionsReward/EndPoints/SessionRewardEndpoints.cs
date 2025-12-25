using Reward_Flow_v2.Rewards.SessionsReward.CreateReward;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.AddEmployeeSessions;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.AddMultipleEmployeeSessions;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.DeleteSessionsReward;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetAllSessionsRewards;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetSessionsRewardById;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetSessionsRewardsByReward;
using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.UpdateSessionsReward;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints;

public static class SessionRewardEndpoints
{
    public static void MapSessionRewardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateSessionsReward();
        app.MapGetAllSessionsRewards();
        app.MapGetSessionsRewardById();
        app.MapGetSessionsRewardsByRewardId();
        app.MapUpdateSessionsReward();
        app.MapDeleteSessionsReward();
        app.MapAddEmployeeSessions();
        app.MapAddMultipleEmployeeSessions();
    }

}