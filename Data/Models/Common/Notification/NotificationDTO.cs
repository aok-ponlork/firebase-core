using Firebase_Auth.Data.Constant;
namespace Firebase_Auth.Data.Models.Common.Notification;
public class NotificationDto : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Destination { get; set; }
    public string? UserId { get; set; }
    public NotificationRecipientType NotificationRecipient { get; set; }
    public NotificationStatus? Status { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? PushOn { get; set; }
}
