using System.ComponentModel.DataAnnotations;
namespace Firebase_Auth.Data.Entities.Authentication
{

    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        // Navigation property for users with this role
        public virtual ICollection<User> Users { get; set; } = [];
        // For more complex permission systems
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }

    // For more granular permissions
    public class Permission : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }

    // Join table for many-to-many relationship
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public Guid PermissionId { get; set; }
        public virtual Permission Permission { get; set; } = null!;
    }
}