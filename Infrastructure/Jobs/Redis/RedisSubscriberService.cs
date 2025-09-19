using StackExchange.Redis;
namespace Firebase_Auth.Infrastructure.Jobs.Redis;

public class RedisSubscriberService : BackgroundService
{
    private readonly IConnectionMultiplexer _redis;
    public RedisSubscriberService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sub = _redis.GetSubscriber();
        sub.Subscribe(RedisChannel.Literal("my-channel"), (channel, message) =>
        {
            Console.WriteLine($"Received: {message}");
        });

        return Task.CompletedTask;
    }
}
