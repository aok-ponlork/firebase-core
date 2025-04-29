using Firebase_Auth.Data.Constant;
namespace Firebase_Auth.Data.Models.Common.Notification;
public class CreateNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Destination { get; set; }
    public string? UserId { get; set; }
    public NotificationRecipientType NotificationRecipient { get; set; }
}
