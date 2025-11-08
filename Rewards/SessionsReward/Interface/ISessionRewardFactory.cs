using Reward_Flow_v2.Common;

namespace Reward_Flow_v2.Rewards.SessionsReward.Interface;

public interface ISessionRewardFactory
{
    public Task<int?> CreateAsync(int createdBy, string name, string? code, int? year, byte? semester, float percentage);
    public Task<SessionRewards?> FindAsync(int sessionRewardId, int CreatedBy);
    public Task<SessionRewards?> FindByRewardIdAsync(int rewardId, int createdBy);
    public Task<bool> UpdateAsync(int sessionRewardId, int createdBy, Optional<string> name = default, Optional<string> code = default, Optional<int?> year = default, Optional<byte?> semester = default, Optional<float> percentage = default);

}

