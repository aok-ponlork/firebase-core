using StackExchange.Redis;
namespace Firebase_Auth.Infrastructure.Jobs.Redis;

public class RedisPublisherService : BackgroundService
{
    private readonly IConnectionMultiplexer _redis;
    public RedisPublisherService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sub = _redis.GetSubscriber();

        while (!stoppingToken.IsCancellationRequested)
        {
            await sub.PublishAsync(RedisChannel.Literal("my-channel"), "Hello at " + DateTime.Now);
            await Task.Delay(5000, stoppingToken);
        }
    }
}
