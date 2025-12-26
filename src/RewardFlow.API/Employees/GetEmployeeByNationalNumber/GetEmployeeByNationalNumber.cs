using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Employees.Data;
using Reward_Flow_v2.Employees.Data.Database;
using System.Security.Claims;
using Reward_Flow_v2.Common.Hashing;

namespace Reward_Flow_v2.Employees.GetEmployeeByNationalNumber;

public static partial class GetEmployeeByNationalNumber
{
    public static void MapGetEmployeeByNationalNumber(this IEndpointRouteBuilder app)
    {
        app.MapGet(EmployeeApiPath.GetByNationalNumber, HandlerAsync)
            .RequireAuthorization()
            .Produces<Employee>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(EmployeeApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(string? nationalNumber, EmployeeDbContext dbContext, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if(currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            if (nationalNumber == null)
                return Results.BadRequest("National number is required");
                
            var nationalNumberHash = XxHasher.Hash(nationalNumber);
            
            var employee = await dbContext.Employee
                .Where(e => e.NationalNumberHash == nationalNumberHash && e.CreatedBy == currentUserId)
                .FirstOrDefaultAsync(cancellationToken);

            return employee == null ? Results.NotFound() : Results.Ok(employee);
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}