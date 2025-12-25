using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class EmployeeRewardConfiguration : IEntityTypeConfiguration<EmployeeReward>
{
    public void Configure(EntityTypeBuilder<EmployeeReward> builder)
    {
        builder.ToTable("employee_rewards");
        builder.HasKey(er => er.Id);
        builder.Property(er => er.Id).ValueGeneratedOnAdd();
        
        builder.Property(er => er.Id).HasColumnName("id");
        builder.Property(er => er.RewardId).HasColumnName("reward_id");
        builder.Property(er => er.EmployeeId).HasColumnName("employee_id");
        builder.Property(er => er.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
    }
}