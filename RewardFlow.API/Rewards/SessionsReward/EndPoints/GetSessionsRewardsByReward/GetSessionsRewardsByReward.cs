using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Dtos;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetSessionsRewardsByReward;

public static class GetSessionsRewardsByReward
{
    public static void MapGetSessionsRewardsByRewardId(this IEndpointRouteBuilder app)
    {
        app.MapGet(RewardApiPath.GetSessionsRewardsByRewardId, HandlerAsync)
            .RequireAuthorization()
            .Produces<SessionRewardDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int rewardId, ISessionRewardFactory factory, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionReward = await factory.FindByRewardIdAsync(rewardId, currentUserId);
            
            if (sessionReward == null)
                return Results.NotFound();

            var dto = new SessionRewardDto(
                sessionReward.SessionRewardId,
                sessionReward.Name,
                sessionReward.Code,
                sessionReward.Year,
                sessionReward.Semester,
                sessionReward.Percentage,
                await sessionReward.GetRewardTotal()
            );

            return Results.Ok(dto);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}