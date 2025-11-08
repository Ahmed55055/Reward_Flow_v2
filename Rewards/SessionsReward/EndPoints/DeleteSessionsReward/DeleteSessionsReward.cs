using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints.DeleteSessionsReward;

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
            var sessionReward = await dbContext.SessionRewardEntity
                .Include(sr => sr.Reward)
                .Where(sr => sr.SessionRewardId == id && sr.Reward.CreatedBy == currentUserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (sessionReward == null)
                return Results.NotFound();

            var subjectSessionRewards = await dbContext.SubjectSessionRewardEntity
                .Include(ssr => ssr.Employees)
                .Where(ssr => ssr.SessionRewardId == id)
                .ToListAsync(cancellationToken);
            
            foreach (var ssr in subjectSessionRewards)
            {
                dbContext.EmployeeSessionRewardEntity.RemoveRange(ssr.Employees);
            }
            
            dbContext.SubjectSessionRewardEntity.RemoveRange(subjectSessionRewards);
            dbContext.SessionRewardEntity.Remove(sessionReward);
            
            var remainingSessionRewards = await dbContext.SessionRewardEntity
                .AnyAsync(sr => sr.RewardId == sessionReward.RewardId, cancellationToken);
            
            if (!remainingSessionRewards)
            {
                var employeeRewards = await dbContext.EmployeeReward
                    .Where(er => er.RewardId == sessionReward.RewardId)
                    .ToListAsync(cancellationToken);
                
                dbContext.EmployeeReward.RemoveRange(employeeRewards);
                dbContext.Reward.Remove(sessionReward.Reward);
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