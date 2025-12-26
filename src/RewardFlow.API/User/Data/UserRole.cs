namespace Reward_Flow_v2.User.Data;
public class UserRole
{
    public int UserRoleId { get; init; }
    public string Name { get; init; }
    public DateTime? CreatedAt { get; init; }
    public bool IsDefault { get; init; }
    public string? Description { get; init; }

    public UserRole(int userRoleId, string name, bool isDefault, string? description)
    {
        UserRoleId = userRoleId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsDefault = isDefault;
        Description = description;
    }     

}
