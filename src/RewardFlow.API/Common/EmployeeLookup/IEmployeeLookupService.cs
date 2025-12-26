namespace Reward_Flow_v2.Common.EmployeeLookup;

public interface IEmployeeLookupService
{
    Task<EmployeeDto?> GetEmployee(int employeeId);
    Task<IEnumerable<EmployeeSalaryDto>> GetEmployeesSalaryById(IEnumerable<int> employeeIds);
}