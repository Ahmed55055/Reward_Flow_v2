using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.User.Data.Database;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.UserRoleId);

        builder.Property(ur => ur.UserRoleId)
            .HasColumnName("user_role_id")
            .ValueGeneratedOnAdd();

        builder.Property(ur => ur.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(ur => ur.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(ur => ur.IsDefault)
            .HasColumnName("is_default")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ur => ur.Description)
            .HasColumnName("description")
            .HasMaxLength(500);
    }
}

