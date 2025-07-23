namespace Firebase_Auth.Infrastructure.MessageQueue.Settings;

public class QueueSettings
{
    public required string HostName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public int Port { get; set; } = 5672;

}
