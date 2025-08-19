using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reward_Flow_v2.User.Data.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Reward_Flow_v2.User.AuthService;

public static class RefreshToken
{
    public record Request(string RefreshToken);
    public record Response(string JWTToken, string RefreshToken);

    public static void MapRefreshToken(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(AuthApiPath.RefreshToken, HandlerAsync)
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(AuthApiPath.Tag);
    }

    private static async Task<IResult> HandlerAsync(Request request, UserDbContext _dbContext, IConfiguration configuration, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken && u.RefreshTokenExpiry > DateTime.UtcNow, cancellationToken);

        if (user == null)
            return Results.Unauthorized();

        var newJwtToken = CreateToken(user, configuration);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.LastVisit = DateTime.UtcNow;
        
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new Response(newJwtToken, newRefreshToken));
    }

    private static string CreateToken(Data.User user, IConfiguration configuration)
    {
        var Claims = new Claim[]
        {
            new Claim (ClaimTypes.NameIdentifier, user.UUID.ToString()),
            new Claim (ClaimTypes.Name, user.Username),
            new Claim (ClaimTypes.Role, Enum.GetName(typeof(Data.UserRoleEnum), user.RoleId)!)
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

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}