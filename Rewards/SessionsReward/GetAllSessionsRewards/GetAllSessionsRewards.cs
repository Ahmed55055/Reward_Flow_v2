using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.GetAllSessionsRewards;

public static class GetAllSessionsRewards
{
    public static void MapGetAllSessionsRewards(this IEndpointRouteBuilder app)
    {
        app.MapGet(RewardApiPath.GetAllSessionsRewards, HandlerAsync)
            .RequireAuthorization()
            .Produces<IEnumerable<Data.EmployeeSessions>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int limit, RewardDbContext dbContext, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionsRewards = await dbContext.SessionsReward
                .Where(sr => sr.SessionReward.CreatedBy == currentUserId)
                .Take(limit > 0 ? limit : 100)
                .ToListAsync(cancellationToken);

            return Results.Ok(sessionsRewards);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}