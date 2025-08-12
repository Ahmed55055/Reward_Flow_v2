namespace Reward_Flow_v2.Common;

public static class AppConfiguration
{
    private static IConfiguration? _config;

    public static void Initialize(IConfiguration configuration)
    {
        _config = configuration;
    }

    public static string? Get(string key)
    {
        return _config?[key];
    }
}
