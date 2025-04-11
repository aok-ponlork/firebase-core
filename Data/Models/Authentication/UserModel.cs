namespace Firebase_Auth.Data.Models.Authentication
{

    public class UserModel : BaseModel
    {
        public string FirebaseUid { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? PhotoUrl { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}