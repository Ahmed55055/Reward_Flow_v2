using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class SessionRewardEntityConfiguration : IEntityTypeConfiguration<SessionRewardEntity>
{
    public void Configure(EntityTypeBuilder<SessionRewardEntity> builder)
    {
        builder.ToTable("session_rewards");
        builder.HasKey(sr => sr.SessionRewardId);
        builder.Property(sr => sr.SessionRewardId).ValueGeneratedOnAdd();
        
        builder.Property(sr => sr.SessionRewardId).HasColumnName("session_reward_id");
        builder.Property(sr => sr.year).HasColumnName("year");
        builder.Property(sr => sr.semester).HasColumnName("Semester");
        builder.Property(sr => sr.Percentage).HasColumnName("percentage");
        builder.Property(sr => sr.RewardId).HasColumnName("reward_id");

        builder.HasOne<RewardEntity>()
            .WithMany()
            .HasForeignKey(sr => sr.RewardId);
    }
}