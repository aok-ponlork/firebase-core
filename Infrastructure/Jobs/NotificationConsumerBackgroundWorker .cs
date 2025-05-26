using System.Text.Json;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Firebase.FCM;
using Firebase_Auth.Infrastructure.MessageQueue.Interface;
namespace Firebase_Auth.Infrastructure.Jobs;

public class NotificationConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqConsumer _consumer;

    public NotificationConsumerService(IServiceScopeFactory scopeFactory, IRabbitMqConsumer consumer)
    {
        _scopeFactory = scopeFactory;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[NOTIFICAITON Consumer] Background service is starting...");
        try
        {
            await _consumer.ConsumeAsync("notification_queue", async (dto) =>
            {
                Console.WriteLine(new string('=', 20));
                Console.WriteLine($"[Consumer] Received message: {dto}");
                Console.WriteLine(new string('=', 20));

                using var scope = _scopeFactory.CreateScope();
                var notificationHelper = scope.ServiceProvider.GetRequiredService<NotificationHelper>();

                // Fix the double encoding issue
                var unwrapped = JsonSerializer.Deserialize<string>(dto);
                var notification = JsonSerializer.Deserialize<SendUserNotificationDto>(unwrapped!);

                if (notification != null)
                {
                    Console.WriteLine($"Parsed Message: {notification.Message}");
                    Console.WriteLine($"Parsed Title: {notification.Title}");
                    Console.WriteLine($"Parsed Token: {notification.DeviceToken}");

                    await notificationHelper.PublishNotificationToUserAsync(notification);
                }
            });

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("[NOTIFICAITON Consumer] Background service is stopping...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"NOTIFICAITON Consumer error: {ex.Message}");
        }
    }
}
