namespace Firebase_Auth.Data.Entities.Common.Notification;
public class NotificationTopic : AuditableEntity
{
    public required string TopicName { get; set; } = string.Empty;
    public string? Description { get; set; }
}