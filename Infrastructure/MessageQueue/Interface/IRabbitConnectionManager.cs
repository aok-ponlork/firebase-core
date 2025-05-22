using RabbitMQ.Client;
namespace Firebase_Auth.Infrastructure.MessageQueue.Interface;

public interface IRabbitConnectionManager
{
    Task<IConnection?> GetConnectionAsync();
}
