namespace Firebase_Auth.Data.Models.Common.Notification;
public class NotificationTopicDto : BaseModel
{
    public required string TopicName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
public class CreateNotificationTopicDto
{
    public required string TopicName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
