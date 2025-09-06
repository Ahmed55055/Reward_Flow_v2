namespace Reward_Flow_v2.Rewards.SubjectSessionReward;

internal interface ISubjectSessionReward
{
    bool AddEmployee(int employeeId);
    bool RemoveEmployee(int employeeId);
    bool Save();

}