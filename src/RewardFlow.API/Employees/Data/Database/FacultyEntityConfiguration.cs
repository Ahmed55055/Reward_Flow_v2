using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Employees.Data.Database;

public class FacultyEntityConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        builder.ToTable("faculties");
        builder.HasKey(f => f.FacultyId);
        builder.Property(f => f.FacultyId).ValueGeneratedOnAdd();
        builder.HasIndex(f => new { f.CreatedBy, f.Name }, "UQ_Faculties_CreatedBy_Name").IsUnique();
        
        builder.Property(f => f.FacultyId).HasColumnName("faculty_id");
        builder.Property(f => f.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(f => f.CreatedBy).HasColumnName("created_by");
        builder.Property(f => f.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        builder.Property(f => f.IsDefault).HasColumnName("is_default");

        // Shadow foreign key - no entity reference to avoid cross-namespace issues
    }
}