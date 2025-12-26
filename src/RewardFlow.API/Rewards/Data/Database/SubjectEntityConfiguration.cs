using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class SubjectEntityConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(s => s.IsTheoretical).HasColumnName("is_theoretical");
        builder.Property(s => s.IsPractical).HasColumnName("is_practical");
        builder.Property(s => s.SubjectPrice).HasColumnName("subject_price").HasColumnType("smallmoney");
    }
}