using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Employees.Data.Database;

public class DepartmentEntityConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        builder.HasKey(d => d.DepartmentId);
        builder.Property(d => d.DepartmentId).ValueGeneratedOnAdd();
        
        builder.Property(d => d.DepartmentId).HasColumnName("department_id");
        builder.Property(d => d.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(d => d.FacultyId).HasColumnName("faculty_id");
        builder.Property(d => d.IsDefault).HasColumnName("is_default");

        builder.HasOne(d => d.Faculty)
            .WithMany(f => f.Departments)
            .HasForeignKey(d => d.FacultyId);
    }
}