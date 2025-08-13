using Scalar.AspNetCore;
using Reward_Flow_v2.Common.ErrorHandling;
using Reward_Flow_v2.User.AuthService;
using Reward_Flow_v2.User.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Reward_Flow_v2.User;
using Reward_Flow_v2.Employees;
using Reward_Flow_v2.Common.UserIdRetrieval;
using Reward_Flow_v2.Common.Tokenization;
using Reward_Flow_v2.Employees.Common;
using Reward_Flow_v2.Common;

namespace Reward_Flow_v2;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddExceptionHandling();
        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();

        // Register services
        AppConfiguration.Initialize(builder.Configuration);
        builder.Services.AddScoped<IUserIdRetrievalService, UserIdRetrievalService>();
        builder.Services.AddScoped<ITokenizer,Tokenizer>();
        builder.Services.AddScoped<IEmployeeTokenService, EmployeeTokenService>();
        
        builder.Services.AddDbContext<UserDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddDbContext<Reward_Flow_v2.Employees.Data.Database.EmployeeDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Token"]!)),
                };
            });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapUsers();
        app.MapEmployeeEndpoints();

        app.Run();
    }
}

