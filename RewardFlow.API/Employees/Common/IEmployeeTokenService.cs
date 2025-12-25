using Reward_Flow_v2.Employees.Data;

namespace Reward_Flow_v2.Employees.Common;

public interface IEmployeeTokenService
{
    Task CreateTokensAsync(Employee employee, int userId, CancellationToken cancellationToken = default);
    Task UpdateTokensAsync(Employee employee, int userId, CancellationToken cancellationToken = default);
    Task DeleteTokensAsync(int employeeId, int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<int>> SearchEmployeesByNameAsync(string searchName, int userId, int limit = 10, CancellationToken cancellationToken = default);
}