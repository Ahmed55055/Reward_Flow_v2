namespace Reward_Flow_v2.Rewards.SessionsReward.Dtos;

public record SessionRewardDto(
    int SessionRewardId,
    string Name,
    string? Code,
    int? Year,
    byte? Semester,
    float Percentage,
    float Total
);