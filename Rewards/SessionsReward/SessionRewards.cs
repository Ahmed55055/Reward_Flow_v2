using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Rewards.Common;
using Reward_Flow_v2.Rewards.Data;
using Reward_Flow_v2.Rewards.Data.Database;
using Reward_Flow_v2.Rewards;
using System.Threading.Tasks;
using Reward_Flow_v2.Common;

namespace Reward_Flow_v2.Rewards.SessionsReward;
public sealed class SessionRewards : Reward
{
    public int SessionRewardId { get => SessionRewardEntity.SessionRewardId; }
    public int? year { get => SessionRewardEntity.year; set => SessionRewardEntity.year = value; }
    public byte? Semester { get => SessionRewardEntity.semester; set => SessionRewardEntity.semester = value; }
    public float Percentage { get => SessionRewardEntity.Percentage; set => SessionRewardEntity.Percentage = value; }
    private SessionRewardEntity SessionRewardEntity { get; set; }

    public SessionRewards(RewardDbContext context, int createdBy, float percentage, string rewardName = "Untitled")
        : base(context, createdBy, (int)RewardTypes.Sessions, rewardName)
    {
        SessionRewardEntity = new SessionRewardEntity();
        Percentage = percentage;
    }

    private SessionRewards(SessionRewardEntity sessionRewardEntity, RewardEntity rewardEntity, RewardDbContext dbContext)
        : base(rewardEntity, dbContext)
    {
        SessionRewardEntity = sessionRewardEntity;
    }
    
    public static async Task<SessionRewards?> Find(int sessionRewardId, RewardDbContext dbContext, int CreatedBy)
    {
        // Check is the same user asking for the reward first to keep privacy code first


        var sessionRewardEntity =
            await dbContext.SessionRewardEntity
            .Include(sr => sr.Reward)
            .Where(sr => sr.SessionRewardId == sessionRewardId && sr.Reward.CreatedBy == CreatedBy)
            .FirstOrDefaultAsync();

        if (sessionRewardEntity is null)
            return null;

        return new SessionRewards(sessionRewardEntity, sessionRewardEntity.Reward, dbContext);
    }

    public override FileStream ToPDF()
    {
        throw new NotImplementedException();
    }

    public override FileStream ToUploadingWorkbook()
    {
        throw new NotImplementedException();
    }

    public override bool UpdateTotal()
    {
        throw new NotImplementedException();
    }

    public override async Task<bool> SaveAsync()
    {
        if (!await base.SaveAsync())
            return false;

        if (Mode == enMode.AddNew)
        {
            SessionRewardEntity.RewardId = this.RewardId;
            await _dbContext.SessionRewardEntity.AddAsync(SessionRewardEntity);
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddSessions(int subjectId, int employeeId, int numberOfStudents)
    {
        var isSubjectExsits =
            await (
            from subject in _dbContext.SubjectSemester
            where subject.SubjectId == subjectId && subject.SemesterNumber == Semester
            select true)
            .AnyAsync();

        SubjectSemester subjectSemester = null;
        if (!isSubjectExsits)
        {
            subjectSemester = await CreateSubjectSemester(subjectId, numberOfStudents);

            if (subjectSemester is null)
                return false;
        }

        var employeeSessions = new EmployeeSessions
        {
            SemesterSubjectId = subjectSemester.SubjectId,
            EmpId = employeeId,
            SessionRewardId = SessionRewardId,
            NumOfSessions = rewardCalculator.CalculateSessions(numberOfStudents)
        };

        _dbContext.EmployeeSessions.Add(employeeSessions);

    }

    private async Task<SubjectSemester?> CreateSubjectSemester(int subjectId, int numberOfStudents)
    {
        SubjectSemester subjectSemester = new SubjectSemester
        {
            SubjectId = subjectId,
            NumberOfStudents = numberOfStudents,
            SemesterNumber = Semester.Value
        };

        _dbContext.SubjectSemester.Add(subjectSemester);
        await _dbContext.SaveChangesAsync();

        return subjectSemester;
    }
}

