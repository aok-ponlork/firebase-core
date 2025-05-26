// using Firebase_Auth.Infrastructure.MessageQueue.Interface;
// namespace Firebase_Auth.Infrastructure.Jobs;

// public class RabbitMqConsumerHostedService : BackgroundService
// {
//     private readonly IRabbitMqConsumer _consumer;
//     public RabbitMqConsumerHostedService(IRabbitMqConsumer consumer)
//     {
//         _consumer = consumer;
//     }
//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         Console.WriteLine("[Consumer] Background service is starting...");
//         try
//         {
//             // Create the consumer once and let it run continuously
//             await _consumer.ConsumeAsync("my_first_queue_name", message =>
//             {
//                 Console.WriteLine(new string('=', 20));
//                 Console.WriteLine($"[Consumer] Received message: {message}");
//                 Console.WriteLine(new string('=', 20));
//             });
//             await Task.Delay(Timeout.Infinite, stoppingToken);
//         }
//         catch (OperationCanceledException)
//         {
//             Console.WriteLine("[Consumer] Background service is stopping...");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Consumer error: {ex.Message}");
//         }
//     }
// }

