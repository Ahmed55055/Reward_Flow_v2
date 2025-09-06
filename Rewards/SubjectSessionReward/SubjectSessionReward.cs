using FluentResults;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards.SubjectSessionReward;

public sealed class SubjectSessionReward : EntityTracker<SubjectSessionRewardEntity>
{
    public int Id { get => Entity.Id; }
    public int SessionRewardId { get => Entity.SessionRewardId; set => Entity.SessionRewardId = value; }
    public int NumberOfSessions { get => Entity.NumberOfSessions; private set => Entity.NumberOfSessions = value; }
    public int SubjectId { get => Entity.SubjectId; set => Entity.SubjectId = value; }
    public int StudentsNumber { get => Entity.StudentsNumber; set => Entity.StudentsNumber = value; }
    public int? MainEmployeeId { get => Entity.MainEmployeeId; set => Entity.MainEmployeeId = value; }
    public int MaxNumberOfEmployees { get => Entity.MaxNumberOfEmployees; set => Entity.MaxNumberOfEmployees = value; }

    private new SubjectSessionRewardEntity Entity { get; set; }
    private new enMode Mode { get; set; }
    private List<EntityTracker<EmployeeSessionRewardEntity>> Employees { get; set; } = new List<EntityTracker<EmployeeSessionRewardEntity>>();

    private readonly RewardDbContext _dbContext;
    private List<EmployeeSessionRewardEntity> RemovedEmployeeSession = new List<EmployeeSessionRewardEntity>();

    public SubjectSessionReward(int sessionRewardId, byte numberOfSessions, int subjectId, int studentsNumber, int mainEmployeeId, RewardDbContext dbContext, byte maxNumberOfEmployees = 3)
    {
        Entity = new SubjectSessionRewardEntity();
        SessionRewardId = sessionRewardId;
        NumberOfSessions = numberOfSessions;
        SubjectId = subjectId;
        StudentsNumber = studentsNumber;
        MainEmployeeId = mainEmployeeId;
        MaxNumberOfEmployees = maxNumberOfEmployees;
        _dbContext = dbContext;
        Mode = enMode.AddNew;
    }

    private SubjectSessionReward(SubjectSessionRewardEntity subjectSessionRewardEntity, RewardDbContext dbContext)
    {
        Entity = subjectSessionRewardEntity;
        Employees =
            subjectSessionRewardEntity.Employees
            .Select(e => new EntityTracker<EmployeeSessionRewardEntity> { Entity = e, Mode = enMode.Update })
            .ToList();

        Mode = enMode.Update;
        _dbContext = dbContext;
    }

    public static SubjectSessionReward Find(int subjectSessionRewardId, RewardDbContext dbContext)
    {
        var subjectSessionReward =
            dbContext.SubjectSessionRewardEntity
            .Include(r => r.Employees)
            .FirstOrDefault(x => x.Id == subjectSessionRewardId);

        if (subjectSessionReward == null)
            return null;

        return new SubjectSessionReward(subjectSessionReward, dbContext);
    }

    public static async Task<bool> Delete(int subjectSessionRewardId, RewardDbContext dbContext)
    {
        var employees = dbContext.EmployeeSessionRewardEntity.Where(x => x.SubjectSessionRewardId == subjectSessionRewardId);
        dbContext.EmployeeSessionRewardEntity.RemoveRange(employees);

        var subject = await dbContext.SubjectSessionRewardEntity.FindAsync(subjectSessionRewardId);
        if (subject != null)
            dbContext.SubjectSessionRewardEntity.Remove(subject);

        return await dbContext.SaveChangesAsync() > 0;
    }

    public bool AddEmployee(int employeeId, bool isMainEmployee = false)
    {
        if (Employees.Any(x => x.Entity.EmployeeId == employeeId))
            return false;

        if (Employees.Count >= MaxNumberOfEmployees)
            return false;

        Employees.Add(new EntityTracker<EmployeeSessionRewardEntity>
        {
            Entity = new EmployeeSessionRewardEntity { EmployeeId = employeeId, SubjectSessionRewardId = this.Id },
            Mode = enMode.AddNew
        });

        if (isMainEmployee)
            MainEmployeeId = employeeId;

        return true;
    }

    public bool[] AddEmployee(List<int> employeeIds)
    {
        return employeeIds.Select(id => AddEmployee(id)).ToArray();
    }

    public bool RemoveEmployee(int employeeId)
    {
        var employee = Employees.Find(x => x.Entity.EmployeeId == employeeId);
        if (employee == null)
            return false;

        if (employee.Mode == enMode.Update)
            RemovedEmployeeSession.Add(employee.Entity);

        Employees.Remove(employee);

        if (MainEmployeeId == employeeId)
            MainEmployeeId = null;

        return true;
    }

    public static IEnumerable<SubjectSessionReward> GetAllForSession(int SessionRewardId, RewardDbContext dbContext)
    {
        return
            dbContext.SubjectSessionRewardEntity
            .Include(x => x.Employees)
            .Where(x => x.SessionRewardId == SessionRewardId)
            .Select(r => new SubjectSessionReward(r, dbContext))
            .ToArray();
    }

    public static async Task<int> SumEmployeeSessions(int employeeId, int SessionRewardId, RewardDbContext dbContext)
    {
        return await (
            from employees in dbContext.EmployeeSessionRewardEntity
            join subjectSessions in dbContext.SubjectSessionRewardEntity
            on employees.SubjectSessionRewardId equals subjectSessions.Id
            where subjectSessions.SessionRewardId == SessionRewardId &&
            employees.Id == employeeId
            select subjectSessions.NumberOfSessions)
            .SumAsync();
    }

    private async Task AddNewSubject()
    {
        await _dbContext.SubjectSessionRewardEntity.AddAsync(Entity);
    }

    private void AddNewEmployees()
    {
        var newEmployees = Employees.Where(x => x.Mode == enMode.AddNew).Select(e => e.Entity).ToArray();
        _dbContext.EmployeeSessionRewardEntity.AddRange(newEmployees);
    }

    private void DeleteRemovedEmployees()
    {
        if (RemovedEmployeeSession.Count == 0)
            return;

        _dbContext.EmployeeSessionRewardEntity.RemoveRange(RemovedEmployeeSession);
        RemovedEmployeeSession.Clear();
    }

    public async Task<bool> Save()
    {
        if (Mode == enMode.AddNew)
        {
            await AddNewSubject();
        }

        AddNewEmployees();
        DeleteRemovedEmployees();
        await _dbContext.SaveChangesAsync();

        Mode = enMode.Update;
        foreach (var emp in Employees)
            if (emp.Entity.Id != 0)
                emp.Mode = enMode.Update;

        return true;
    }
}