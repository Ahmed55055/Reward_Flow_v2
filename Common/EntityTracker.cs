using FluentResults;
using Reward_Flow_v2.Rewards.Data.Database;
using System.Threading.Tasks;

namespace Reward_Flow_v2.Common;

public class EntityTracker<T>
{
    public enMode Mode { get; set; }
    public T Entity {  get; set; }
}
