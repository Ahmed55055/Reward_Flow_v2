using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Reward_Flow_v2.Employees.Data.Database;
using Reward_Flow_v2.Rewards.Data.Database;
using Reward_Flow_v2.User.Data.Database;

namespace Reward_Flow_v2;

public static class DatabasesMigrations
{
    public static void EnsureDatabasesCreated(this IEndpointRouteBuilder builder)
    {
        var Contexts = new DbContext[]
        {
            builder.ServiceProvider.GetRequiredService<UserDbContext>(),
            builder.ServiceProvider.GetRequiredService<EmployeeDbContext>(),
            builder.ServiceProvider.GetRequiredService<RewardDbContext>()
        };

        foreach (DbContext dbContext in Contexts)
        {
            // Runs only first time for first dbcontext to create the database and create the tables of the selected dbcontext
            if (dbContext.Database.IsSqlServer() && !dbContext.Database.CanConnect())
                dbContext.Database.EnsureCreated();
            else
            {
                // This method used because all the dbContexts and tables are at the same database right now 
                // so tables created explicitly 
                var creator = dbContext.GetService<IRelationalDatabaseCreator>();
                creator.CreateTables();
            }
        }
    }
}