namespace Reward_Flow_v2.Rewards.SessionsReward.Dtos;

public record EmployeeSessionDto
{
    public required int SubjectId { get; init; }
    public required EmployeeDto Employee { get; init; }
    public required int NumberOfStudents { get; init; }
    public required bool IsMainEmployee { get; init; }
}
