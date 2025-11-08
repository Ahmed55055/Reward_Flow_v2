using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EmployeeLookup;
using Reward_Flow_v2.Employees.Data;
using Reward_Flow_v2.Rewards;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;
using Reward_Flow_v2.Rewards.SessionsReward.Common;
using Reward_Flow_v2.Rewards.SessionsReward.Dtos;
using Reward_Flow_v2.Rewards.SessionsReward.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards.SessionsReward;
public sealed partial class SessionRewards : Reward, ISessionReward
{
    public int SessionRewardId { get => SessionRewardEntity.SessionRewardId; }
    public int? year { get => SessionRewardEntity.year; }
    public byte? Semester { get => SessionRewardEntity.semester; }
    public float Percentage { get => SessionRewardEntity.Percentage; }
    private SessionRewardEntity SessionRewardEntity { get; set; }
    private readonly ISessionRewardCalculator rewardCalculator;
    private readonly ISessionRewardRules rules;
    private readonly IDbContextFactory<RewardDbContext> contextFactory;
    private readonly IEmployeeLookupService employeeLookup;

    private SessionRewards(IDbContextFactory<RewardDbContext> contextFactory, ISessionRewardCalculator rewardCalculator, ISessionRewardRules rules, IEmployeeLookupService employeeLookup, int createdBy)
        : base(contextFactory.CreateDbContext(), createdBy)
    {
        this.contextFactory = contextFactory;
        this.employeeLookup = employeeLookup;
        SessionRewardEntity = new SessionRewardEntity();
        this.rewardCalculator = rewardCalculator;
        this.rules = rules;
        InitializeNew(createdBy, (int)RewardTypes.Sessions);
    }

    private SessionRewards(IDbContextFactory<RewardDbContext> contextFactory, SessionRewardEntity sessionRewardEntity, ISessionRewardCalculator rewardCalculator, ISessionRewardRules rules, IEmployeeLookupService employeeLookup, RewardEntity rewardEntity)
        : base(contextFactory.CreateDbContext(), rewardEntity)
    {
        this.contextFactory = contextFactory;
        this.employeeLookup = employeeLookup;
        SessionRewardEntity = sessionRewardEntity;
        this.rewardCalculator = rewardCalculator;
        this.rules = rules;
    }
    public override FileStream ToPDF()
    {
        throw new NotImplementedException();
    }

    public override FileStream ToUploadingWorkbook()
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateTotal()
    {
        using var context = contextFactory.CreateDbContext();

        var employeeSessionData = await GetEmployeesTotalSessionsAsync(context);

        var employeeIds = employeeSessionData.Select(e => e.EmployeeId);
        var employeeSalaries = await GetEmployeesSalariesAsync(employeeIds);


        var employeeRewards = await context.EmployeeReward
            .Where(er => er.RewardId == this.RewardId && !er.IsUpdated)
            .ToListAsync();

        foreach (var empData in employeeSessionData)
        {
            employeeSalaries.TryGetValue(empData.EmployeeId, out float salary);

            var allowedSessions = GetAllowedSessions(empData.TotalSessions);
            var total = rewardCalculator.CalculateTotal(allowedSessions, salary, this.Percentage);

            UpdateOrCreateEmployeeReward(context, employeeRewards, empData, total);
        }

        await context.SaveChangesAsync();
    }

    private async Task<bool> Save()
    {
        try
        {
            await base.SaveAsync();

            if (Mode == enMode.AddNew)
            {
                SessionRewardEntity.RewardId = this.RewardId;
                await _dbContext.SessionRewardEntity.AddAsync(SessionRewardEntity);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AssignEmployeeToSubjectAsync(SessionSubjectDto dto)
    {
        try
        {
            using var context = contextFactory.CreateDbContext();
            var subjectSessionReward = await GetOrCreateSubjectSessionReward(dto, context);
            subjectSessionReward.AddEmployees(dto.Employees.Select(e => e.EmployeeId));
            await MarkEmployeeRewardsAsOutdated(dto.Employees.Select(e => e.EmployeeId), context);
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            using var context = contextFactory.CreateDbContext();
            var subjectSessionReward = await GetOrCreateSubjectSessionReward(dto, context);
            subjectSessionReward.AddEmployees(dto.Employees.Select(e => e.EmployeeId));
            await MarkEmployeeRewardsAsOutdated(dto.Employees.Select(e => e.EmployeeId), context);
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AssignEmployeeToSubjectAsync(IEnumerable<SessionSubjectDto> dtos)
    {
        try
        {
            using var context = contextFactory.CreateDbContext();
            var allEmployeeIds = dtos.SelectMany(dto => dto.Employees.Select(e => e.EmployeeId)).Distinct();
            foreach (var dto in dtos)
            {
                var subjectSessionReward = await GetOrCreateSubjectSessionReward(dto, context);
                subjectSessionReward.AddEmployees(dto.Employees.Select(e => e.EmployeeId));
            }
            await MarkEmployeeRewardsAsOutdated(allEmployeeIds, context);
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            using var context = contextFactory.CreateDbContext();
            var allEmployeeIds = dtos.SelectMany(dto => dto.Employees.Select(e => e.EmployeeId)).Distinct();
            foreach (var dto in dtos)
            {
                var subjectSessionReward = await GetOrCreateSubjectSessionReward(dto, context);
                subjectSessionReward.AddEmployees(dto.Employees.Select(e => e.EmployeeId));
            }
            await MarkEmployeeRewardsAsOutdated(allEmployeeIds, context);
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<SubjectSessionReward> GetOrCreateSubjectSessionReward(SessionSubjectDto dto, RewardDbContext context)
    {
        var SSR = await SubjectSessionReward.FindBySubjectAndSession(dto.SubjectId, this.Semester.Value, this.SessionRewardId, context);

        if (SSR is not null)
            return SSR;

        var subjectSessionReward = new SubjectSessionReward(
            this.SessionRewardId,
            (byte)rewardCalculator.CalculateSessions(dto.NumberOfStudents),
            dto.SubjectId,
            dto.NumberOfStudents,
            dto.Employees.First().EmployeeId,
            context
        );

        return subjectSessionReward;
    }

    private bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException?.Message is not string message)
            return false;

        message = message.ToLowerInvariant();

        return message.Contains("unique constraint") ||
               message.Contains("unique index") ||
               message.Contains("duplicate key") ||
               message.Contains("duplicate entry");
    }

    public record EmployeeSessionData(int EmployeeId, int TotalSessions);
    
    void UpdateOrCreateEmployeeReward(RewardDbContext context, List<EmployeeReward> employeeRewards, EmployeeSessionData empData, float total)
    {
        var empReward = employeeRewards.FirstOrDefault(er => er.EmployeeId == empData.EmployeeId);
        if (empReward == null)
        {
            empReward = new EmployeeReward
            {
                RewardId = this.RewardId,
                EmployeeId = empData.EmployeeId,
                Total = total,
                IsUpdated = true
            };
            context.EmployeeReward.Add(empReward);
        }
        else
        {
            empReward.Total = total;
            empReward.IsUpdated = true;
        }
    }

    async Task<IEnumerable<EmployeeSessionData>> GetEmployeesTotalSessionsAsync(RewardDbContext context)
    {
        return await (
            from empSession in context.EmployeeSessionRewardEntity
            join subjectSession in context.SubjectSessionRewardEntity on empSession.SubjectSessionRewardId equals subjectSession.Id
            join empReward in context.EmployeeReward on empSession.EmployeeId equals empReward.EmployeeId into empRewardGroup
            from empReward in empRewardGroup.DefaultIfEmpty()
            where subjectSession.SessionRewardId == this.SessionRewardId && !empReward.IsUpdated
            group new { empSession.EmployeeId, subjectSession.NumberOfSessions } by empSession.EmployeeId into g
            select new EmployeeSessionData
            (
                g.Key,
                g.Sum(x => x.NumberOfSessions)
            )
        ).ToListAsync();
    }

    async Task<Dictionary<int, float>> GetEmployeesSalariesAsync(IEnumerable<int> employeeIds)
    {
        return (await
            employeeLookup.GetEmployeesSalaryById(employeeIds))
            .ToDictionary(e => e.EmployeeId, e => e.Salary);
    }

    private int GetAllowedSessions(int totalSessions)
    {
        return rules.IsWithInMaximumNumberOfSession(totalSessions)
                ? totalSessions
                : rules.MaxSessionsNumber;
    }

    private async Task MarkEmployeeRewardsAsOutdated(IEnumerable<int> employeeIds, RewardDbContext context)
    {
        var employeeRewards = await context.EmployeeReward
            .Where(er => er.RewardId == this.RewardId && employeeIds.Contains(er.EmployeeId))
            .ToListAsync();

        foreach (var empReward in employeeRewards)
        {
            empReward.IsUpdated = false;
        }
    }

    public async Task<EmployeeRewardDto?> GetEmployeeReward(int employeeId)
    {
        using var context = contextFactory.CreateDbContext();
        var employeeReward = await context.EmployeeReward
            .Where(er => er.RewardId == this.RewardId && er.EmployeeId == employeeId)
            .FirstOrDefaultAsync();
            
        return employeeReward is not null 
            ? new EmployeeRewardDto(employeeReward.Id, employeeReward.EmployeeId, employeeReward.Total)
            : null;
    }

    public async Task<IEnumerable<EmployeeRewardDto>> GetEmployeesReward()
    {
        using var context = contextFactory.CreateDbContext();
        return await context.EmployeeReward
            .Where(er => er.RewardId == this.RewardId)
            .Select(er => new EmployeeRewardDto(er.Id, er.EmployeeId, er.Total))
            .ToListAsync();
    }

    public async Task<float> GetRewardTotal()
    {
        using var context = contextFactory.CreateDbContext();
        return await context.EmployeeReward
            .Where(er => er.RewardId == this.RewardId)
            .SumAsync(er => er.Total);
    }
}

