using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common.EmployeeLookup;
using Reward_Flow_v2.Employees.Data.Database;

namespace Reward_Flow_v2.Employees.Shared;

public class EmployeeLookupService : IEmployeeLookupService
{
    private readonly EmployeeDbContext _dbContext;

    public EmployeeLookupService(EmployeeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EmployeeDto?> GetEmployee(int employeeId)
    {
        var employee = await _dbContext.Employee
            .Where(e => e.EmployeeId == employeeId)
            .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Salary = e.Salary,
                    NationalNumber = e.NationalNumber
                })
            .FirstOrDefaultAsync();

        return employee;
    }

    public async Task<IEnumerable<EmployeeSalaryDto>> GetEmployeesSalaryById(IEnumerable<int> employeeIds)
    {
        var employees = await _dbContext.Employee
            .Where(e => employeeIds.Contains(e.EmployeeId))
            .Select(e => new EmployeeSalaryDto
            {
                EmployeeId = e.EmployeeId,
                Salary = e.Salary ?? 0
            })
            .ToListAsync();

        return employees;
    }
}