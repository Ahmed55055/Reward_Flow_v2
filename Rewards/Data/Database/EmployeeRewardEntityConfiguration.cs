using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class EmployeeRewardEntityConfiguration : IEntityTypeConfiguration<EmployeeReward>
{
    public void Configure(EntityTypeBuilder<EmployeeReward> builder)
    {
        builder.ToTable("employee_rewards");
        builder.HasKey(er => er.Id);
        builder.Property(er => er.Id).ValueGeneratedOnAdd();
        
        builder.Property(er => er.Id).HasColumnName("id");
        builder.Property(er => er.RewardId).HasColumnName("reward_id");
        builder.Property(er => er.EmpId).HasColumnName("emp_id");
        builder.Property(er => er.Total).HasColumnName("total").HasColumnType("smallmoney");

        builder.HasOne(er => er.Reward)
            .WithMany(r => r.EmployeeRewards)
            .HasForeignKey(er => er.RewardId);

        builder.HasOne(er => er.Employee)
            .WithMany()
            .HasForeignKey(er => er.EmpId)
            .HasPrincipalKey(e => e.EmployeeId);
    }
}