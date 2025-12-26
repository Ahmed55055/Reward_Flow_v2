namespace Reward_Flow_v2.Rewards.SessionsReward.Dtos;

public record SessionSubjectDto
{
    public required int SubjectId { get; init; }
    public required int NumberOfStudents { get; init; }
    public required int MainEmployeeId { get; init; }
    public required IEnumerable<EmployeeDto> Employees { get; init; }
}