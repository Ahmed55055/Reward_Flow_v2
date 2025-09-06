using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.GetSessionsRewardsByReward;

public static class GetSessionsRewardsByReward
{
    public static void MapGetSessionsRewardsByReward(this IEndpointRouteBuilder app)
    {
        app.MapGet(RewardApiPath.GetSessionsRewardsByReward, HandlerAsync)
            .RequireAuthorization()
            .Produces<IEnumerable<Data.EmployeeSessions>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int rewardId, RewardDbContext dbContext, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionsRewards = await dbContext.SessionsReward
                .Where(sr => sr.SessionRewardId == rewardId && sr.SessionReward.CreatedBy == currentUserId)
                .ToListAsync(cancellationToken);

            return Results.Ok(sessionsRewards);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}