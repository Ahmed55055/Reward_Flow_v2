using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class SessionsRewardEntityConfiguration : IEntityTypeConfiguration<EmployeeSessions>
{
    public void Configure(EntityTypeBuilder<EmployeeSessions> builder)
    {
        builder.ToTable("sessions_rewards");
        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.Id).ValueGeneratedOnAdd();
        
        builder.Property(sr => sr.Id).HasColumnName("id");
        builder.Property(sr => sr.SemesterSubjectId).HasColumnName("semester_subject_id");
        builder.Property(sr => sr.NumOfSessions).HasColumnName("num_of_sessions");
        builder.Property(sr => sr.EmpId).HasColumnName("emp_id");
        builder.Property(sr => sr.Total).HasColumnName("total").HasColumnType("smallmoney");
        builder.Property(sr => sr.SessionRewardId).HasColumnName("reward_id");

        builder.HasOne(sr => sr.SemesterSubject)
            .WithMany(ss => ss.SessionsRewards)
            .HasForeignKey(sr => sr.SemesterSubjectId);

        builder.HasOne(sr => sr.Employee)
            .WithMany()
            .HasForeignKey(sr => sr.EmpId)
            .HasPrincipalKey(e => e.EmployeeId);

        builder.HasOne(sr => sr.SessionReward)
            .WithMany(r => r.SessionsRewards)
            .HasForeignKey(sr => sr.SessionRewardId);
    }
}