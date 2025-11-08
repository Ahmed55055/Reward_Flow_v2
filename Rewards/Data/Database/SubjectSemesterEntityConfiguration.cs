using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reward_Flow_v2.Rewards.Data.Database;

public class SubjectSemesterEntityConfiguration : IEntityTypeConfiguration<SemesterSubject>
{
    public void Configure(EntityTypeBuilder<SemesterSubject> builder)
    {
        builder.ToTable("subject_semesters");
        builder.HasKey(ss => ss.Id);
        builder.Property(ss => ss.Id).ValueGeneratedOnAdd();
        
        builder.Property(ss => ss.Id).HasColumnName("id");
        builder.Property(ss => ss.SubjectId).HasColumnName("subject_id");
        builder.Property(ss => ss.SemesterNumber).HasColumnName("semester_number");
        builder.Property(ss => ss.NumberOfStudents).HasColumnName("number_of_students");

        builder.HasOne(ss => ss.Subject)
            .WithMany(s => s.SubjectSemesters)
            .HasForeignKey(ss => ss.SubjectId);
    }
}