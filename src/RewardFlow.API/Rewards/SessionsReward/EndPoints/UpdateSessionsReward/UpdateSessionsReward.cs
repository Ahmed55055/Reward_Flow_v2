using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EndpointValidation;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;

namespace Reward_Flow_v2.Rewards.SessionsReward.EndPoints.UpdateSessionsReward;

public static class UpdateSessionsReward
{


    public static void MapUpdateSessionsReward(this IEndpointRouteBuilder app)
    {
        app.MapPatch(RewardApiPath.UpdateSessionsReward, HandlerAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag)
            .Validation(new UpdateSessionsRewardValidator());
    }

    private static async Task<IResult> HandlerAsync(int id, UpdateSessionsRewardRequest request, ISessionRewardFactory factory, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);

        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var success = await factory.UpdateAsync(
                id, 
                currentUserId, 
                request.RewardName, 
                request.RewardCode, 
                request.Year, 
                request.Semester, 
                request.Percentage);

            return success ? Results.NoContent() : Results.NotFound();
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}