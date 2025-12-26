using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Employees.Data.Database;

public class EmployeeStatusEntityConfiguration : IEntityTypeConfiguration<EmployeeStatus>
{
    public void Configure(EntityTypeBuilder<EmployeeStatus> builder)
    {
        builder.ToTable("employee_status");
        builder.HasKey(s => s.StatusId);
        builder.Property(s => s.StatusId).ValueGeneratedOnAdd();
        
        builder.Property(s => s.StatusId).HasColumnName("status_id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(255);
    }
}