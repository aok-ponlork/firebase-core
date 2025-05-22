using Firebase_Auth.Infrastructure.MessageQueue.Interface;
using Firebase_Auth.Infrastructure.MessageQueue.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

public class RabbitConnectionManager(IOptions<QueueSettings> options) : IRabbitConnectionManager
{
    private IConnection? _connection;
    private readonly QueueSettings _settings = options.Value;
    public async Task<IConnection?> GetConnectionAsync()
    {
        if (_connection != null && _connection.IsOpen)
            return _connection;
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password,
                Port = _settings.Port
            };
            _connection = await factory.CreateConnectionAsync();
            return _connection;
        }
        catch (Exception ex)
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("[RabbitMQ] Connection failed:");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");

            var inner = ex.InnerException;
            int level = 1;
            while (inner != null)
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"Inner Exception Level {level}: {inner.Message}");
                Console.WriteLine(inner.StackTrace);
                inner = inner.InnerException;
                level++;
            }
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Connection Payload value Hostname: {_settings.HostName}");
            Console.WriteLine($"Connection Payload value Username: {_settings.UserName}");
            Console.WriteLine($"Connection Payload value Password: {_settings.Password}");
            Console.WriteLine($"Connection Payload value Port: {_settings.Port}");
            Console.WriteLine(new string('-', 50));
            return null;
        }
    }
}
