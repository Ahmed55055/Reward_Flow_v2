//using Reward_Flow_v2.Rewards.SessionsReward.CreateReward;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.UpdateSessionsReward;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetSessionsRewardsByReward;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetAllSessionsRewards;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.DeleteSessionsReward;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetSessionsRewardById;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.AddEmployeeSessions;
//using Reward_Flow_v2.Rewards.SessionsReward.EndPoints.AddMultipleEmployeeSessions;

//namespace Reward_Flow_v2.Rewards;

//public static class RewardEndpoints
//{
//    public static void MapRewardEndpoints(this IEndpointRouteBuilder app)
//    {
//        app.MapCreateSessionsReward();
//        app.MapGetAllSessionsRewards();
//        app.MapGetSessionsRewardById();
//        app.MapUpdateSessionsReward();
//        app.MapDeleteSessionsReward();
//        app.MapGetSessionsRewardsByRewardId();
//        app.MapAddEmployeeSessions();
//        app.MapAddMultipleEmployeeSessions();
//    }
//}