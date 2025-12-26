namespace Reward_Flow_v2.User.Data;

public class Plan
{
    public int Id { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastEdit { get; init; }
    public string? Description { get; init; }
    public string? Summary { get; init; }

    public Plan(int id, string name, DateTime? lastEdit, string? description, string? summary)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        LastEdit = lastEdit;
        Description = description;
        Summary = summary;
    }
}