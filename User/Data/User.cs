using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Reward_Flow_v2.User.Data
{
    public sealed class User
    {
        public int Id { get; private set; }
        public Guid UUID { get; private set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public UserRole? UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastVisit { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int PlanId { get; set; }
        public Plan? Plan { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public User()
        {
            UUID = Guid.NewGuid();

            RoleId = ((int)UserRoleEnum.User);
            UserRole = null;
            CreatedAt = DateTime.Now;
            LastVisit = DateTime.Now;
            IsActive = true;
            ProfilePictureUrl = null;
            PlanId = ((int)PlanEnum.Free);
            Plan = null;
        }
        private User(Guid userId,
                     string username,
                     string password,
                     string? email,
                     int roleId,
                     UserRole? userRole,
                     DateTime createdAt,
                     DateTime? lastVisit,
                     bool isActive,
                     string? profilePictureUrl,
                     int planId,
                     Plan? plan)
        {
            UUID = userId;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = password ?? throw new ArgumentNullException(nameof(password));
            Email = email;
            RoleId = roleId;
            UserRole = userRole;
            CreatedAt = createdAt;
            LastVisit = lastVisit;
            IsActive = isActive;
            ProfilePictureUrl = profilePictureUrl;
            PlanId = planId;
            Plan = plan;
        }

        internal static User PrepareNewUser(
            string username,
            string password,
            string? email,
            int roleId,
            bool isActive = true,
            string? profilePictureUrl = null,
            int planId = 0)
        {
            return new User(
                userId: Guid.NewGuid(),
                username: username,
                password: password,
                email: email,
                roleId: roleId,
                userRole: null, // Navigation property can be set later
                createdAt: DateTime.Now, // Matches SQL default GETDATE()
                lastVisit: DateTime.Now, // Matches SQL default GETDATE()
                isActive: isActive,
                profilePictureUrl: profilePictureUrl, // Matches SQL default NULL
                planId: planId, // Matches SQL default 0
                plan: null); // Navigation property can be set later
        }

        internal void PrepareNewUser(string username, string hashedPassword, string? email)
        {
            this.UUID = Guid.NewGuid();
            this.Username = username;
            this.PasswordHash = hashedPassword;
            this.Email = email;

            RoleId = ((int)UserRoleEnum.User);
            UserRole = null;
            CreatedAt = DateTime.Now;
            LastVisit = DateTime.Now;
            IsActive = true;
            ProfilePictureUrl = null;
            PlanId = ((int)PlanEnum.Free);
            Plan = null;
        }
    }
}