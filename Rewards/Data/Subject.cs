namespace Reward_Flow_v2.Rewards.Data;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsTheoretical { get; set; }
    public bool IsPractical { get; set; }
    public float SubjectPrice { get; set; }
    
    public virtual ICollection<SubjectSemester> SubjectSemesters { get; set; } = new List<SubjectSemester>();
}