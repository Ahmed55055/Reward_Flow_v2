namespace Reward_Flow_v2.User.Data.Database;

using Microsoft.EntityFrameworkCore;
using System.Data;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options,IConfiguration configuration) : DbContext(options)
{
    private const string Schema = "dbo";
    public DbSet<User> User => Set<User>();
    public DbSet<UserRole> Role => Set<UserRole>();
    public DbSet<Plan> Plan => Set<Plan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(schema: Schema);
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PlanEntityConfiguration());

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole(1, "admin",false, "Administrator role"),
            new UserRole(2, "guest", true, "Guest role"),
            new UserRole(3, "user", false, "Regular user role")
        );

        modelBuilder.Entity<Plan>().HasData(
            new Plan(1, "free", null, "Free plan", "Basic features"),
            new Plan(2, "Premium", null, "Premium plan", "All features included")
        );

    }
}