using Microsoft.AspNetCore.Http;
using Reward_Flow_v2.Common.UserIdRetrieval;
using System.Security.Claims;

namespace Reward_Flow_v2.Common;

public static class GetHttpsAccessorUserId
{
    public static string? GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
    {
        return
            httpContextAccessor
            .HttpContext?
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
            .Value;
    }

    public static Guid GetCurrentUserGuidId(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = GetCurrentUserId(httpContextAccessor);

        if (userId is null)
            return Guid.Empty;

        Guid GuidId;

        if (!Guid.TryParse(userId, out GuidId))
            return Guid.Empty;

        return GuidId;
    }

    public static async Task<int> GetCurrentUserIntIdAsync(this IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken = default)
    {
        var userGuidId = GetCurrentUserGuidId(httpContextAccessor);
        if (userGuidId == Guid.Empty) return 0;
        
        var userIdService = httpContextAccessor.HttpContext?.RequestServices.GetService<IUserIdRetrievalService>();
        return userIdService != null ? await userIdService.GetUserIntIdAsync(userGuidId, cancellationToken) : 0;
    }
}
