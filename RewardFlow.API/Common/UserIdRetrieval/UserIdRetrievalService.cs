using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.User.Data.Database;

namespace Reward_Flow_v2.Common.UserIdRetrieval;

public class UserIdRetrievalService : IUserIdRetrievalService
{
    private readonly UserDbContext _dbContext;

    public UserIdRetrievalService(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetUserIntIdAsync(Guid userGuid, CancellationToken cancellationToken = default)
    {
        return await _dbContext.User
            .Where(u => u.UUID == userGuid)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}