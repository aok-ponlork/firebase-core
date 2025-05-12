namespace Firebase_Auth.Helper.Firebase.FCM;

using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Services.Interfaces;
using FirebaseAdmin.Messaging;

public class NotificationHelper
{
    private readonly INotificationService _notificationManager;
    public NotificationHelper(INotificationService notificationService)
    {
        _notificationManager = notificationService;
    }
    public async Task SendUserNotificationAsync(SendUserNotificationDto model)
    {
        var message = new Message
        {
            Token = model.DeviceToken,
            Data = new Dictionary<string, string>
            {
                { "title", model.Title },
                { "message", model.Message },
                { "destination", model.Destination ?? "" },
                { "notificationRecipient", model.RecipientType.ToString() },
                { "imageUrl", model.ImageUrl ?? "" }
            },
            Notification = new Notification
            {
                Title = model.Title,
                Body = model.Message,
                ImageUrl = model.ImageUrl
            }
        };

        var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        Console.WriteLine($"Successfully sent message: {result}");
    }

    public async Task SendGeneralNotificationAsync(NotificationDto model)
    {
        var message = new Message
        {
            Topic = "general-notification",
            Data = new Dictionary<string, string>
            {
                { "title", model.Title },
                { "message", model.Message },
                { "destination", model.Destination ?? "" },
                { "notificationRecipient", model.NotificationRecipient.ToString() },
                { "imageUrl", model.ImageUrl ?? "" }
            },
            Notification = new Notification
            {
                Title = model.Title,
                Body = model.Message,
                ImageUrl = model.ImageUrl
            }
        };

        var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        Console.WriteLine($"Successfully sent general message: {result}");
    }
}
