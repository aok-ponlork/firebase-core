using Firebase_Auth.Data.Constant;

namespace Firebase_Auth.Data.Models.Common.Notification;

public abstract class BaseNotificationDto
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? ImageUrl { get; set; }
    public string? Destination { get; set; }
    public NotificationRecipientType RecipientType { get; set; }
}

public class CreateGeneralNotificationDto : BaseNotificationDto
{
    public string? UserId { get; set; }

    public CreateGeneralNotificationDto(string title, string message, string? userId, NotificationRecipientType recipientType)
    {
        Title = title;
        Message = message;
        UserId = userId;
        RecipientType = recipientType;
    }
}

public class CreateUserNotificationDto : BaseNotificationDto
{
    public required string UserId { get; set; }

    public CreateUserNotificationDto(string title, string message, string userId, NotificationRecipientType recipientType)
    {
        Title = title;
        Message = message;
        UserId = userId;
        RecipientType = recipientType;
    }
}

public class SendUserNotificationDto : CreateUserNotificationDto
{
    public required string DeviceToken { get; set; }

    public SendUserNotificationDto(string title, string message, string userId, NotificationRecipientType recipientType, string deviceToken)
        : base(title, message, userId, recipientType)
    {
        DeviceToken = deviceToken;
    }
}
