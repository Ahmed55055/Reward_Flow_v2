
using FluentResults;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Reward_Flow_v2.User.Data;
using Reward_Flow_v2.User.Data.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Reward_Flow_v2.User.AuthService.Login;

public static class Login
{
    public record Request(string username, string password);
    public record Response(string JWTToken);

    public static void MapUserLogin(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(AuthApiPath.Login, HandlerAsync)
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces<IEnumerable<FluentValidation.Results.ValidationFailure>>(StatusCodes.Status400BadRequest)
            .WithTags(AuthApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(Request request, UserDbContext _dbContext, IConfiguration configuration, CancellationToken cancellationToken)
    {
        var validationResult = new LoginUserRequestValidator().Validate(request);
        
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var user = await _dbContext.User            
            .FirstOrDefaultAsync(u => u.Username == request.username, cancellationToken);

        if (user == null)
            return Results.BadRequest("Invalid username or password");

        var passwordHasher = new PasswordHasher<Data.User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.password);

        if (result == PasswordVerificationResult.Failed)
            return Results.BadRequest("Invalid username or password");

        var token = CreateToken(user, configuration);

        user.LastVisit = DateTime.UtcNow;
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new Response(token));
    }

    private static string CreateToken(Data.User user, IConfiguration configuration)
    {
        var Claims = new Claim[]
        {
            new Claim (ClaimTypes.NameIdentifier, user.UUID.ToString()),
            new Claim (ClaimTypes.Name, user.Username),
            new Claim (ClaimTypes.Role, Enum.GetName(typeof( UserRoleEnum),user.RoleId)!)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescripter = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("JWT:Issuer"),
            audience: configuration.GetValue<string>("JWT:Audience"),
            claims: Claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescripter);
    }
}
