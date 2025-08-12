namespace Reward_Flow_v2.Common.UserIdRetrieval;

public interface IUserIdRetrievalService
{
    Task<int> GetUserIntIdAsync(Guid userGuid, CancellationToken cancellationToken = default);
}