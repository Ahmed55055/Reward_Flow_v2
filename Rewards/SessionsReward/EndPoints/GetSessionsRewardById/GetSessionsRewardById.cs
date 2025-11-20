using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Dtos;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints.GetSessionsRewardById;

public static class GetSessionsRewardById
{
    public static void MapGetSessionsRewardById(this IEndpointRouteBuilder app)
    {
        app.MapGet(RewardApiPath.GetSessionsRewardById, HandlerAsync)
            .RequireAuthorization()
            .Produces<SessionRewardDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int id, ISessionRewardFactory factory, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionsReward = await factory.FindAsync(id, currentUserId);

            if (sessionsReward == null)
                return Results.NotFound();

            var dto = new SessionRewardDto(
                sessionsReward.SessionRewardId,
                sessionsReward.Name,
                sessionsReward.Code,
                sessionsReward.year,
                sessionsReward.Semester,
                sessionsReward.Percentage,
                await sessionsReward.GetRewardTotal()
            );

            return Results.Ok(dto);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}