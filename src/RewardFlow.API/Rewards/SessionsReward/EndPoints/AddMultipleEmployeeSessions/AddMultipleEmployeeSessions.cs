using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Dtos;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints.AddMultipleEmployeeSessions;

public static class AddMultipleEmployeeSessions
{
    public static void MapAddMultipleEmployeeSessions(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{RewardApiPath.SessionsReward}/{{id}}/employees/batch", HandlerAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int id, IEnumerable<SessionSubjectDto> dtos, ISessionRewardFactory factory, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionReward = await factory.FindAsync(id, currentUserId);
            
            if (sessionReward == null)
                return Results.NotFound();

            await sessionReward.AssignEmployeeToSubjectAsync(dtos);
            
            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}