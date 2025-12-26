using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards;


public abstract class Reward : IReward
{
    public int RewardId { get => Entity.Id; }
    public string Name { get => Entity.Name; set => Entity.Name = value; }
    public float Total { get => Entity.Total; protected set => Entity.Total = value; }
    public DateTime CreatedAt { get => Entity.CreatedAt; protected set => Entity.CreatedAt = value; }
    public DateTime LastUpdate { get => Entity.LastUpdate; protected set => Entity.LastUpdate = value; }
    public int CreatedBy { get => Entity.CreatedBy; protected set => Entity.CreatedBy = value; }
    public string? Code { get => Entity.Code; set => Entity.Code = value; }
    public int RewardType { get => Entity.RewardType; set => Entity.RewardType = value; }
    public int NumberOfEmployees { get => Entity.NumberOfEmployees; set => Entity.NumberOfEmployees = value; }
    public EntityState State => _dbContext.Entry(Entity).State;

    protected RewardEntity Entity { get; set; }
    private readonly RewardDbContext _dbContext;


    protected Reward(RewardDbContext dbContext, int createdBy, int rewardType, string name = "Untitled")
    {
        _dbContext = dbContext;
        Entity = new RewardEntity();
        CreatedBy = createdBy;
        RewardType = rewardType;
        Name = name;
        
        CreatedAt = DateTime.Now;
        LastUpdate = DateTime.Now;
        Total = 0;
        NumberOfEmployees = 0;
        
        dbContext.Entry(Entity).State = EntityState.Added;
    }

    protected Reward(RewardDbContext dbContext, RewardEntity entity)
    {
        _dbContext = dbContext;
        this.Entity = entity;
        dbContext.Entry(Entity).State = EntityState.Unchanged;
    }

    public abstract Task UpdateTotal();
    public abstract FileStream ToPDF();
    public abstract FileStream ToUploadingWorkbook();

    public virtual async Task<bool> SaveAsync()
    {
        try
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            return false;
        }
    }
}

