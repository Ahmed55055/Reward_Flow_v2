namespace Reward_Flow_v2.User;

internal static class AuthApiPath
{
    public const string Tag = "Auth";
    private const string AuthRootApi = $"{ApiPath.Route}/{Tag}";

    public const string Register = $"{AuthRootApi}/Register";
    public const string Login = $"{AuthRootApi}/login";
    public const string RefreshToken = $"{AuthRootApi}/refresh-token";
}
