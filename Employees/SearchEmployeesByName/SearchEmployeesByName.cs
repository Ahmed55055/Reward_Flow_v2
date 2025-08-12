using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Employees.Common;
using Reward_Flow_v2.Employees.Data;
using Reward_Flow_v2.Employees.Data.Database;

namespace Reward_Flow_v2.Employees.SearchEmployeesByName;

public static class SearchEmployeesByName
{
    public static void MapSearchEmployeesByName(this IEndpointRouteBuilder app)
    {
        app.MapGet(EmployeeApiPath.SearchByName, HandlerAsync)
            .RequireAuthorization()
            .Produces<IEnumerable<Employee>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(EmployeeApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(string name, int limit, EmployeeDbContext dbContext, IEmployeeTokenService tokenService, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if(currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var employeeIds = await tokenService.SearchEmployeesByNameAsync(name, currentUserId, limit, cancellationToken);
            
            var employees = await dbContext.Employee
                .Where(e => employeeIds.Contains(e.EmployeeId) && e.CreatedBy == currentUserId)
                .ToListAsync(cancellationToken);

            return Results.Ok(employees);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}