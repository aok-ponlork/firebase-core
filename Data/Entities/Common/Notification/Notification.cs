using Firebase_Auth.Data.Constant;
namespace Firebase_Auth.Data.Entities.Common.Notification;
public class Notification : AuditableEntity
{

    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public string? Destination { get; private set; }
    public string? UserId { get; private set; }
    public NotificationRecipientType NotificationRecipient { get; private set; }
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;
    public DateTime? PushOn { get; private set; }

    // Method
    public void MarkAsPushed()
    {
        PushOn = DateTime.UtcNow;
        Status = NotificationStatus.Pushed;
    }
}
