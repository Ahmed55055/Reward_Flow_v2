namespace Reward_Flow_v2.Rewards;

public interface IReward
{
    public int RewardId { get;  }
    public string Name { get; set; }
    public float Total { get; }
    public DateTime CreatedAt { get;  }
    public DateTime LastUpdate { get;  }
    public int CreatedBy { get;  }
    public string? Code { get; set; }
    public int RewardType { get; set; }
    public int NumberOfEmployees { get; set; }

    public Task<bool> SaveAsync();
}