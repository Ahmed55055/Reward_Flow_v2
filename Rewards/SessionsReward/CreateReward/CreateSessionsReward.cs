using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EndpointValidation;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;

namespace Reward_Flow_v2.Rewards.SessionsReward.CreateReward;

public static partial class CreateSessionsReward
{
    public record Request(string? RewardName, string? RewardCode, int? Year, byte? Semester, float Percentage);

    public static void MapCreateSessionsReward(this IEndpointRouteBuilder app)
    {
        app.MapPost(RewardApiPath.CreateSessionsReward, HandlerAsync)
            .RequireAuthorization()
            .Produces<Data.EmployeeSessions>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag)
            .Validation(new CreateRewardRequestValidator());
    }

    private static async Task<IResult> HandlerAsync(Request request, RewardDbContext dbContext, IRewardCalculator calculator, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);

        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {

            var sessionReward = new SessionRewards(dbContext, currentUserId, request.Percentage, request.RewardName)
            {
                Code = request.RewardCode,
                year = request.Year,
                Semester = request.Semester
            };

            if (!await sessionReward.SaveAsync())
                return Results.UnprocessableEntity();

            return Results.Created(RewardApiPath.GetSessionsRewardById, sessionReward.SessionRewardId);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}