using Firebase_Auth.Infrastructure.MessageQueue.Interface;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IRabbitConnectionManager _connectionManager;

    public RabbitMqPublisher(IRabbitConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        var connection = await _connectionManager.GetConnectionAsync();
        if (connection == null || !connection.IsOpen)
        {
            Console.WriteLine("Failed to publish: RabbitMQ connection unavailable.");
            return;
        }
        using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
    }
}
