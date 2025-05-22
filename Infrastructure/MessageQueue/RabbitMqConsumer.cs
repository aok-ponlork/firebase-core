using Firebase_Auth.Infrastructure.MessageQueue.Interface;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumer : IRabbitMqConsumer
{
    private readonly IRabbitConnectionManager _connectionManager;
    public RabbitMqConsumer(IRabbitConnectionManager connection)
    {
        _connectionManager = connection;
    }
    public async Task ConsumeAsync(string queueName, Action<string> onMessageReceived)
    {
        var connection = await _connectionManager.GetConnectionAsync();
        if (connection == null || !connection.IsOpen)
        {
            Console.WriteLine("Failed to consume: RabbitMQ connection unavailable.");
            return;
        }
        var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            onMessageReceived(message);
            await Task.CompletedTask;
        };
        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

    }
}