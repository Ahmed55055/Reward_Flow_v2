using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EndpointValidation;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.UpdateSessionsReward;

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

    private static async Task<IResult> HandlerAsync(int id, UpdateSessionsRewardRequest request, RewardDbContext dbContext, IRewardCalculator calculator, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);

        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var sessionReward = await SessionRewards.Find(id, dbContext, currentUserId);

            if (sessionReward is null)
                return Results.NotFound();

            UpdateValues(sessionReward, request);
            
            return (await sessionReward.SaveAsync())?
             Results.NoContent():
             Results.UnprocessableEntity();

        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }

    private static void UpdateValues(SessionRewards reward, UpdateSessionsRewardRequest request)
    {
        if (request.RewardName.HasValue) reward.Name = request.RewardName.Value;
        if (request.RewardCode.HasValue) reward.Code = request.RewardCode.Value;
        if (request.Year.HasValue) reward.year = request.Year.Value;
        if (request.Semester.HasValue) reward.Semester = request.Semester.Value;
        if (request.Percentage.HasValue) reward.Percentage = request.Percentage.Value;
    }
}