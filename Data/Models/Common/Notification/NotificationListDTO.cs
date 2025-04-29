using Firebase_Auth.Data.Constant;
namespace Firebase_Auth.Data.Models.Common.Notification;
public class NotificationListDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Destination { get; set; }
    public NotificationStatus? Status { get; set; }
    public DateTime? PushOn { get; set; }
}
