using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Employees.Data;
using Reward_Flow_v2.Employees.Data.Database;

namespace Reward_Flow_v2.Employees.GetAllEmployees;

public static class GetAllEmployees
{
    public static void MapGetAllEmployees(this IEndpointRouteBuilder app)
    {
        app.MapGet(EmployeeApiPath.GetAll, HandlerAsync)
            .RequireAuthorization()
            .Produces<IEnumerable<Employee>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(EmployeeApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(int limit, EmployeeDbContext dbContext, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if(currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var employees = await dbContext.Employee
                .Where(e => e.CreatedBy == currentUserId)
                .Take(limit > 0 ? limit : 100)
                .ToListAsync(cancellationToken);

            return Results.Ok(employees);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}