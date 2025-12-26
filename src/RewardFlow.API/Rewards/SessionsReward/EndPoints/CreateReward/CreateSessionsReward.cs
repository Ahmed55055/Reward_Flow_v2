using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EndpointValidation;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Dtos;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;

namespace Reward_Flow_v2.Rewards.SessionsReward.CreateReward;

public static partial class CreateSessionsReward
{
    public record Request(string? RewardName, string? RewardCode, int? Year, byte? Semester, float Percentage);

    public static void MapCreateSessionsReward(this IEndpointRouteBuilder app)
    {
        app.MapPost(RewardApiPath.CreateSessionsReward, HandlerAsync)
            .RequireAuthorization()
            .Produces<SessionRewardDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RewardApiPath.Tag)
            .Validation(new CreateRewardRequestValidator());
    }

    private static async Task<IResult> HandlerAsync(Request request, ISessionRewardFactory factory, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);

        if (currentUserId == 0)
            return Results.Unauthorized();

        try
        {

            var sessionRewardId = await factory.CreateAsync(
                currentUserId,
                request.RewardName ?? "Untitled",
                request.RewardCode,
                request.Year,
                request.Semester,
                request.Percentage);

            if (sessionRewardId == null)
                return Results.UnprocessableEntity();

            return Results.Created(RewardApiPath.GetSessionsRewardById, sessionRewardId);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}