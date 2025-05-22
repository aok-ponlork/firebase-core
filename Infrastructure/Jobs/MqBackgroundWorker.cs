using Firebase_Auth.Infrastructure.MessageQueue.Interface;
public class RabbitMqConsumerHostedService : BackgroundService
{
    private readonly IRabbitMqConsumer _consumer;
    public RabbitMqConsumerHostedService(IRabbitMqConsumer consumer)
    {
        _consumer = consumer;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[Consumer] Background service is starting...");
        await _consumer.ConsumeAsync("my_first_queue_name", message =>
        {
            Console.WriteLine(new string('=', 20));
            Console.WriteLine($"[Consumer] Received message: {message}");
            Console.WriteLine(new string('=', 20));
        });
        // Keep the service alive until cancellation is requested
        try
        {
            Console.WriteLine("[Consumer] Background service is running...");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("[Consumer] Background service is stopping...");
        }
    }
}

