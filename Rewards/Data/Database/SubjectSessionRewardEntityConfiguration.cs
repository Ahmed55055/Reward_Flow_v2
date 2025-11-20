using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class SubjectSessionRewardEntityConfiguration : IEntityTypeConfiguration<SubjectSessionRewardEntity>
{
    public void Configure(EntityTypeBuilder<SubjectSessionRewardEntity> builder)
    {
        builder.ToTable("subject_session_rewards");
        builder.HasKey(ssr => ssr.Id);
        builder.Property(ssr => ssr.Id).ValueGeneratedOnAdd();
        
        builder.Property(ssr => ssr.Id).HasColumnName("id");
        builder.Property(ssr => ssr.SessionRewardId).HasColumnName("session_reward_id");
        builder.Property(ssr => ssr.NumberOfSessions).HasColumnName("number_of_sessions");
        builder.Property(ssr => ssr.SemesterSubjectId).HasColumnName("subject_id");
        builder.Property(ssr => ssr.StudentsNumber).HasColumnName("students_number");
        builder.Property(ssr => ssr.MainEmployeeId).HasColumnName("main_employee_id");
        builder.Property(ssr => ssr.MaxNumberOfEmployees).HasColumnName("max_number_of_employees");

        builder.HasMany(ssr => ssr.Employees)
            .WithOne()
            .HasForeignKey(e => e.SubjectSessionRewardId);

        builder.HasOne<SemesterSubject>()
            .WithMany()
            .HasForeignKey(ssr => ssr.SemesterSubjectId);
    }
}