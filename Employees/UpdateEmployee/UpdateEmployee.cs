using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common;
using Reward_Flow_v2.Common.EndpointValidation;
using Reward_Flow_v2.Employees.Common;
using Reward_Flow_v2.Employees.Data.Database;
using System.Security.Claims;

namespace Reward_Flow_v2.Employees.UpdateEmployee;

public static partial class UpdateEmployee
{
    //public struct IsUpdated<T>
    //{
    //    public bool IsSet;
    //    public T Value;
    //}
    //public record reqest
    //{
    //    IsUpdated<string?> Name;
    //    IsUpdated<string?> NationalNumber;
    //}
    public record Request(string? Name, string? NationalNumber, string? AccountNumber, float? Salary, int? FacultyId, int? DepartmentId, byte? JobTitle, byte? Status);

    public static void MapUpdateEmployee(this IEndpointRouteBuilder app)
    {
        app.MapPatch(EmployeeApiPath.Update, HandlerAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<IEnumerable<FluentValidation.Results.ValidationFailure>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(EmployeeApiPath.Tag)
            .Validation(new UpdateEmployeeRequestValidator());
    }

    private static async Task<IResult> HandlerAsync(int id, Request request, EmployeeDbContext dbContext, IEmployeeTokenService tokenService, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var currentUserId = await httpContextAccessor.GetCurrentUserIntIdAsync(cancellationToken);
        
        if(currentUserId == 0)
            return Results.Unauthorized();

        try
        {
            var employee = await dbContext.Employee
                .Where(e => e.EmployeeId == id && e.CreatedBy == currentUserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (employee == null)
                return Results.NotFound();

            bool nameChanged = request.Name != null && employee.Name != request.Name;
            
            if (request.Name != null) employee.Name = request.Name;
            if (request.NationalNumber != null) employee.NationalNumber = request.NationalNumber;
            if (request.AccountNumber != null) employee.AccountNumber = request.AccountNumber;
            if (request.Salary.HasValue) employee.Salary = request.Salary;
            if (request.FacultyId.HasValue) employee.FacultyId = request.FacultyId;
            if (request.DepartmentId.HasValue) employee.DepartmentId = request.DepartmentId;
            if (request.JobTitle.HasValue) employee.JobTitle = request.JobTitle;
            if (request.Status.HasValue) employee.Status = request.Status;

            await dbContext.SaveChangesAsync(cancellationToken);
            
            if (nameChanged)
            {
                await tokenService.UpdateTokensAsync(employee, currentUserId, cancellationToken);
            }
            
            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.InternalServerError();
        }
    }
}