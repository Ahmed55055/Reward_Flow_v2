using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards;


public abstract class Reward : EntityTracker<RewardEntity>, IReward
{
    public int RewardId { get => Entity.Id; }
    public string Name { get => Entity.Name; set => Entity.Name = value; }
    public float Total { get => Entity.Total; private set => Entity.Total = value; }
    public DateTime CreatedAt { get => Entity.CreatedAt; private set => Entity.CreatedAt = value; }
    public DateTime LastUpdate { get => Entity.LastUpdate; private set => Entity.LastUpdate = value; }
    public int CreatedBy { get => Entity.CreatedBy; private set => Entity.CreatedBy = value; }
    public string? Code { get => Entity.Code; set => Entity.Code = value; }
    public int RewardType { get => Entity.RewardType; set => Entity.RewardType = value; }
    public int NumberOfEmployees { get => Entity.NumberOfEmployees; set => Entity.NumberOfEmployees = value; }

    protected readonly RewardDbContext _dbContext;


    protected Reward(RewardDbContext dbContext, int createdBy, int rewardType, string name = "Untitled")
    {
        Entity = new RewardEntity();
        CreatedBy = createdBy;
        _dbContext = dbContext;
        RewardType = rewardType;
        Name = name;
        CreatedAt = DateTime.Now;
        LastUpdate = DateTime.Now;
        Total = 0;
        NumberOfEmployees = 0;
        Mode = enMode.AddNew;
    }

    protected Reward(RewardEntity rewardEntity, RewardDbContext dbContext)
    {
        Entity = rewardEntity;
        _dbContext = dbContext;
        Mode = enMode.Update;
    }

    public abstract Task<TReward> Find<TReward>(int rewardId) where TReward : Reward;
    public abstract bool UpdateTotal();
    public abstract FileStream ToPDF();
    public abstract FileStream ToUploadingWorkbook();

    public virtual async Task<bool> SaveAsync()
    {
        if (Mode == enMode.AddNew)
        {
            await _dbContext.Reward.AddAsync(Entity);
        }

        await _dbContext.SaveChangesAsync();
        Mode = enMode.Update;
        return true;
    }
}

