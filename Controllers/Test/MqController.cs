using Firebase_Auth.Infrastructure.MessageQueue.Interface;
using Microsoft.AspNetCore.Mvc;
namespace Firebase_Auth.Controllers.Test;

[Route("api/message-queue")]

public class MqController(IRabbitMqPublisher publisher, ILogger<CoreController> logger) : CoreController(logger)
{
    private readonly IRabbitMqPublisher _publisher = publisher;

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] string message)
    {
        await _publisher.PublishAsync(message, "my_first_queue_name");
        return Ok("Message sent!");
    }
}