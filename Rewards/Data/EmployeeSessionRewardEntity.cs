using FluentResults;
using Reward_Flow_v2.Rewards.Data.Database;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Rewards.Data;

public class EmployeeSessionRewardEntity
{
    public int Id { get; set; }
    public required int SubjectSessionRewardId { get; set; }
    public required int EmployeeId { get; set; }
}
