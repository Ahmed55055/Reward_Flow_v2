namespace Reward_Flow_v2.Rewards.Common;

public enum RewardTypes { Sessions = 1, Control = 2 }

public static class RewardTypeConverter
{
    public static int ToInt(this RewardTypes e) => (int)e;
}

