using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Employees.Data.Database;

public class JobTitleEntityConfiguration : IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("job_titles");
        builder.HasKey(j => j.JobTitleId);
        builder.Property(j => j.JobTitleId).ValueGeneratedOnAdd();
        builder.HasIndex(j => j.Name, "UQ__job_title__72E12F1B42005F0D").IsUnique();
        
        builder.Property(j => j.JobTitleId).HasColumnName("job_title_id");
        builder.Property(j => j.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(j => j.Description).HasColumnName("description").HasMaxLength(255);
    }
}