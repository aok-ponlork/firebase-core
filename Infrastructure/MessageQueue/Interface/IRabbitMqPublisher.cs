namespace Firebase_Auth.Infrastructure.MessageQueue.Interface;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message, string queueName);
}

