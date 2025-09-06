using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.Data.Database;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards.Data;

public class SessionRewardEntity
{
    public int SessionRewardId { get; set; }
    public int? year { get; set; }
    public byte? semester { get; set; }
    public float Percentage { get; set; }
    public int RewardId { get; set; }

    public virtual RewardEntity Reward { get; set; } = null!;
}

