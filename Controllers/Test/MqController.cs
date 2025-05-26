using System.Text.Json;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Infrastructure.MessageQueue.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Firebase_Auth.Controllers.Test;

[Route("api/message-queue")]
//[Authorize]
public class MqController(IRabbitMqPublisher publisher, ILogger<CoreController> logger) : CoreController(logger)
{
    private readonly IRabbitMqPublisher _publisher = publisher;

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromQuery] string queueName, [FromBody] SendUserNotificationDto model)
    {
        var json = JsonSerializer.Serialize(model);
        //publish work into queue
        await _publisher.PublishAsync(json, queueName);
        return Ok("Notification enqueued!");
    }


    [HttpPost("send-random")]
    public IActionResult SendRandomQueueMessage()
    {
        // Start the task, but don't wait for it
        _ = Task.Run(async () => await _publisher.PublishRandomMessagesAsync("my_first_queue_name", 10, 2000));
        return Ok("Random message sending started!");
    }

}