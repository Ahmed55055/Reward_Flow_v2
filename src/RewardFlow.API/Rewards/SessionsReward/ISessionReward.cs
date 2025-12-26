using Reward_Flow_v2.Rewards.SessionsReward.Dtos;

namespace Reward_Flow_v2.Rewards.SessionsReward;

public record EmployeeRewardDto(int EmployeeRewardId,int EmployeeId,float Total);

internal interface ISessionReward
{
    public Task<bool> AssignEmployeeToSubjectAsync(SessionSubjectDto dto);
    public Task<bool> AssignEmployeeToSubjectAsync(IEnumerable<SessionSubjectDto> dto);
    public Task<EmployeeRewardDto?> GetEmployeeReward(int employeeId);
    public Task<IEnumerable<EmployeeRewardDto>> GetEmployeesReward();
    public Task<float> GetRewardTotal();
}