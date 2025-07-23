namespace Firebase_Auth.Infrastructure.MessageQueue.Interface;
public interface IRabbitMqConsumer
{
    Task ConsumeAsync(string queueName, Func<string, Task> onMessageReceived);
}
