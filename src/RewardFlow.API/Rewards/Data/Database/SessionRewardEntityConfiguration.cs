using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class SessionRewardEntityConfiguration : IEntityTypeConfiguration<SessionRewardEntity>
{
    public void Configure(EntityTypeBuilder<SessionRewardEntity> builder)
    {
        builder.ToTable("session_rewards");
        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.Id).ValueGeneratedNever();
        
        builder.Property(sr => sr.Id).HasColumnName("session_reward_id");
        builder.Property(sr => sr.year).HasColumnName("Year");
        builder.Property(sr => sr.semester).HasColumnName("Semester");
        builder.Property(sr => sr.Percentage).HasColumnName("percentage");

        builder.HasOne<RewardEntity>(sr => sr.Reward)
            .WithOne()
            .HasForeignKey<SessionRewardEntity>(sr => sr.Id)
            .IsRequired();
    }
}