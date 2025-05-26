namespace Firebase_Auth.Infrastructure.MessageQueue.Interface;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message, string queueName);
    Task PublishRandomMessagesAsync(string queueName, int messageCount, int delayMs = 1000);
}

