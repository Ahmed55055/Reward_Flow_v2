using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Dtos;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints.AddEmployeeSessions;

public static class AddEmployeeSessions
{
    public static void MapAddEmployeeSessions(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{RewardApiPath.SessionsReward}/{{id}}/employees", HandlerAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int id, SessionSubjectDto dto, ISessionRewardFactory factory, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionReward = await factory.FindAsync(id, currentUserId);
            
            if (sessionReward == null)
                return Results.NotFound();

            await sessionReward.AssignEmployeeToSubjectAsync(dto);
            
            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}