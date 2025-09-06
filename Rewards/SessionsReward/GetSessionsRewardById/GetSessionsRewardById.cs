using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.GetSessionsRewardById;

public static class GetSessionsRewardById
{
    public static void MapGetSessionsRewardById(this IEndpointRouteBuilder app)
    {
        app.MapGet(RewardApiPath.GetSessionsRewardById, HandlerAsync)
            .RequireAuthorization()
            .Produces<Data.EmployeeSessions>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int id, RewardDbContext dbContext, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionsReward = await dbContext.SessionsReward
                .Where(sr => sr.Id == id && sr.SessionReward.CreatedBy == currentUserId)
                .FirstOrDefaultAsync(cancellationToken);

            return sessionsReward == null ? Results.NotFound() : Results.Ok(sessionsReward);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}