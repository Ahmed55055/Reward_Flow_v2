using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.DeleteSessionsReward;

public static class DeleteSessionsReward
{
    public static void MapDeleteSessionsReward(this IEndpointRouteBuilder app)
    {
        app.MapDelete(RewardApiPath.DeleteSessionsReward, HandlerAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
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

            if (sessionsReward == null)
                return Results.NoContent();

            var rewardId = sessionsReward.SessionRewardId;
            
            dbContext.SessionsReward.Remove(sessionsReward);
            
            var remainingSessionsRewards = await dbContext.SessionsReward
                .AnyAsync(sr => sr.SessionRewardId == rewardId, cancellationToken);
            
            if (!remainingSessionsRewards)
            {
                var employeeRewards = await dbContext.EmployeeReward
                    .Where(er => er.RewardId == rewardId)
                    .ToListAsync(cancellationToken);
                
                dbContext.EmployeeReward.RemoveRange(employeeRewards);
                
                var reward = await dbContext.Reward.FindAsync(rewardId, cancellationToken);
                if (reward != null)
                    dbContext.Reward.Remove(reward);
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}