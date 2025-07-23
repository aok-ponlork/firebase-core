using Firebase_Auth.Data.Constant;

namespace Firebase_Auth.Data.Models.Common.Notification;

public abstract class BaseNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Destination { get; set; }
    public NotificationRecipientType RecipientType { get; set; }
}

public class CreateGeneralNotificationDto : BaseNotificationDto
{

    public CreateGeneralNotificationDto(string title, string message, NotificationRecipientType recipientType)
    {
        Title = title;
        Message = message;
        RecipientType = recipientType;
    }
}

public class CreateUserNotificationDto : BaseNotificationDto
{
    public string UserId { get; set; }

    public CreateUserNotificationDto(string title, string message, string userId, NotificationRecipientType recipientType)
    {
        Title = title;
        Message = message;
        UserId = userId;
        RecipientType = recipientType;
    }
}

//dto for user notification req
public class SendUserNotificationDto : CreateUserNotificationDto
{
    public string DeviceToken { get; set; }

    public SendUserNotificationDto(string title, string message, string userId, NotificationRecipientType recipientType, string deviceToken)
        : base(title, message, userId, recipientType)
    {
        DeviceToken = deviceToken;
    }
}
//dto for topic notification req
public class SendTopicNotificationDto : CreateGeneralNotificationDto
{
    public required string Topic { get; set; }

    public SendTopicNotificationDto(string title, string message, NotificationRecipientType recipientType, string topic)
        : base(title, message, recipientType)
    {
        Topic = topic;
    }
}