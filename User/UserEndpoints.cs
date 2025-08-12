using Reward_Flow_v2.User.AuthService.Login;
using Reward_Flow_v2.User.AuthService.Register;

namespace Reward_Flow_v2.User;

internal static class UserEndpoints
{
    internal static void MapUsers(this IEndpointRouteBuilder builder)
    {
        builder.MapRegisterUser();
        builder.MapUserLogin();
    }
}
