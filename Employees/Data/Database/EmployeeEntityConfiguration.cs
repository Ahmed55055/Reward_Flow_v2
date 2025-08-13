using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reward_Flow_v2.Common.Encryption;

namespace Reward_Flow_v2.Employees.Data.Database;

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");
        builder.HasKey(e => e.EmployeeId);
        builder.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
        
        builder.Property(e => e.EmployeeId).HasColumnName("employee_id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(255)
            .HasConversion(
                v => AesEncryptionService.EncryptString(v),
                v => AesEncryptionService.DecryptString(v));
        
        builder.Property(e => e.NationalNumber).HasColumnName("national_number").HasMaxLength(255).IsUnicode(false)
            .HasConversion(
                v => AesEncryptionService.EncryptString(v),
                v => AesEncryptionService.DecryptString(v));
        
        builder.Property(e => e.AccountNumber).HasColumnName("account_number").HasMaxLength(20).IsUnicode(false)
            .HasConversion(
                v => AesEncryptionService.EncryptString(v),
                v => AesEncryptionService.DecryptString(v));
        builder.Property(e => e.Salary).HasColumnName("salary").IsRequired(false);
        builder.Property(e => e.FacultyId).HasColumnName("faculty_id");
        builder.Property(e => e.DepartmentId).HasColumnName("department_id");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.JobTitle).HasColumnName("job_title");
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.Status).HasColumnName("status");

        builder.ToTable("employees", t =>
            t.HasCheckConstraint("CHK_Salary_NonNegative", "[salary] >= 0 OR [salary] IS NULL"));
        
        builder.HasOne<User.Data.User>()
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .HasPrincipalKey(u=>u.Id);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);

        builder.HasOne(e => e.Faculty)
            .WithMany(f => f.Employees)
            .HasForeignKey(e => e.FacultyId);

        builder.HasOne(e => e.JobTitleNavigation)
            .WithMany(j => j.Employees)
            .HasForeignKey(e => e.JobTitle);

        builder.HasOne(e => e.StatusNavigation)
            .WithMany(s => s.Employees)
            .HasForeignKey(e => e.Status);
    }
}