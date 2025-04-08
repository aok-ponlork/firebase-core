using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Firebase_Auth.Data.Entities.Authentication
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(128)]
        //Should add indexing,
        public string FirebaseUid { get; set; } = string.Empty;
        // User identity information
        [StringLength(256)]
        public string UserName { get; set; } = string.Empty;
        [StringLength(256)]
        public string? Email { get; set; }
        [StringLength(50)]
        public string? PhoneNumber { get; set; }
        [StringLength(256)]
        public string? FullName { get; set; }
        [StringLength(2048)]
        public string? PhotoUrl { get; set; }
        public Guid? RoleId { get; set; }
        
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
        // Timestamps
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public DateTime? LastLoginAt { get; set; }
        // Account status
        public bool IsActive { get; set; } = true;
        // Additional properties
        [Column(TypeName = "jsonb")]
        public string? AdditionalData { get; set; }
    }
}