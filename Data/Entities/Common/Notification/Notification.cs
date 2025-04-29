
using Firebase_Auth.Data.Constant;
namespace Firebase_Auth.Data.Entities.Common;
public class Notification : AuditableEntity
{
    public Notification(string title, string message, string? userId, NotificationRecipientType recipientType)
    {
        Title = title;
        Message = message;
        UserId = userId;
        NotificationRecipient = recipientType;
        CreatedOn = DateTime.UtcNow;
        Status = NotificationStatus.Pending;
    }

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
