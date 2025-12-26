using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EmployeeLookup;
using Reward_Flow_v2.Rewards;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;
using Reward_Flow_v2.Rewards.SessionsReward.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards.SessionsReward;
public sealed partial class SessionRewards
{

    public sealed class SessionRewardFactory : ISessionRewardFactory
    {
        private readonly ISessionRewardCalculator rewardCalculator;
        private readonly ISessionRewardRules rules;
        private readonly IDbContextFactory<RewardDbContext> contextFactory;
        private readonly IEmployeeLookupService employeeLookup;

        public SessionRewardFactory(ISessionRewardCalculator rewardCalculator, ISessionRewardRules rules, IDbContextFactory<RewardDbContext> contextFactory, IEmployeeLookupService employeeLookup)
        {
            this.rewardCalculator = rewardCalculator;
            this.rules = rules;
            this.contextFactory = contextFactory;
            this.employeeLookup = employeeLookup;
        }

        public async Task<int?> CreateAsync(int createdBy, string name, string? code, int? year, byte? semester, float percentage)
        {
            var sessionReward = new SessionRewards(contextFactory, rewardCalculator, rules, employeeLookup, createdBy);
            sessionReward.Name = name;
            sessionReward.Code = code;
            sessionReward.SessionRewardEntity.year = year;
            sessionReward.SessionRewardEntity.semester = semester;
            sessionReward.SessionRewardEntity.Percentage = percentage;
            
            var success = await sessionReward.Save();
            return success ? sessionReward.SessionRewardId : null;
        }

        public async Task<SessionRewards?> FindAsync(int sessionRewardId, int CreatedBy)
        {
            using var context = contextFactory.CreateDbContext();
            var sessionRewardEntity =
                await context.SessionRewardEntity
                .Include(sr => sr.Reward)
                .FirstOrDefaultAsync(sr => sr.Id == sessionRewardId && sr.Reward.CreatedBy == CreatedBy);

            if (sessionRewardEntity is null)
                return null;

            return new SessionRewards(contextFactory, sessionRewardEntity, rewardCalculator, rules, employeeLookup, sessionRewardEntity.Reward);
        }

        public async Task<SessionRewards?> FindByRewardIdAsync(int rewardId, int createdBy)
        {
            using var context = contextFactory.CreateDbContext();
            var sessionRewardEntity =
                await context.SessionRewardEntity
                .Include(sr => sr.Reward)
                .FirstOrDefaultAsync(sr => sr.Id == rewardId && sr.Reward.CreatedBy == createdBy);

            if (sessionRewardEntity is null)
                return null;

            return new SessionRewards(contextFactory, sessionRewardEntity, rewardCalculator, rules, employeeLookup, sessionRewardEntity.Reward);
        }

        public async Task<bool> UpdateAsync(int sessionRewardId, int createdBy, Optional<string> name = default, Optional<string> code = default, Optional<int?> year = default, Optional<byte?> semester = default, Optional<float> percentage = default)
        {
            var sessionReward = await FindAsync(sessionRewardId, createdBy);
            if (sessionReward == null) return false;

            if (name.HasValue) sessionReward.Name = name.Value;
            if (code.HasValue) sessionReward.Code = code.Value;
            if (year.HasValue) sessionReward.SessionRewardEntity.year = year.Value;
            if (semester.HasValue) sessionReward.SessionRewardEntity.semester = semester.Value;
            if (percentage.HasValue) sessionReward.SessionRewardEntity.Percentage = percentage.Value;

            if (sessionReward.State == EntityState.Unchanged)
                return true;
            
            return await sessionReward.Save();
        }
    }
}

