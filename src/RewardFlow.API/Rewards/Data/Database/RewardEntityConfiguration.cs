using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class RewardEntityConfiguration : IEntityTypeConfiguration<RewardEntity>
{
    public void Configure(EntityTypeBuilder<RewardEntity> builder)
    {
        builder.ToTable("rewards");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(r => r.Total).HasColumnName("total").HasColumnType("smallmoney");
        builder.Property(r => r.CreatedAt).HasColumnName("created_at").HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        builder.Property(r => r.LastUpdate).HasColumnName("last_update").HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        builder.Property(r => r.CreatedBy).HasColumnName("created_by");
        builder.Property(r => r.Code).HasColumnName("code").HasMaxLength(50);
        builder.Property(r => r.RewardType).HasColumnName("reward_type");
        builder.Property(r => r.NumberOfEmployees).HasColumnName("number_of_employees");


    }
}