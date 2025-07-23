using Firebase_Auth.Common.Filters;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Firebase.FCM;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Common.Notification;
[Route("api/notification")]
//[Authorize(Roles = "admin")]
public class NotificationController : CoreController
{
    private readonly INotificationService _service;
    private readonly NotificationHelper _notificationHelper;
    public NotificationController(ILogger<CoreController> logger, INotificationService service, NotificationHelper notificationHelper) : base(logger)
    {
        _service = service;
        _notificationHelper = notificationHelper;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ListGeneralNotificationAsync([FromQuery] FilterRequest filter)
    {
        try
        {
            var response = await _service.GetGeneralNotificationForClientPagination(filter);
            return ToSuccess("Success! ", response);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpPost("topic-notification")]
    public async Task<IActionResult> CreatNotificationGeneralAsync([FromBody] CreateGeneralNotificationDto payload)
    {
        try
        {
            var response = await _service.CreateGeneralNotificationAsync(payload);
            return ToSuccess("Success Create general notificaiton.", response);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");

        }
    }
    [HttpPost("user-notification")]
    public async Task<IActionResult> CreatUserNotificationAsync([FromBody] CreateUserNotificationDto payload)
    {
        try
        {
            var response = await _service.CreateNotificationForSpecificUserAsync(payload);
            return ToSuccess("Success Create general notificaiton.", response);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");

        }
    }
    [HttpPost("push-to-user")]
    public async Task<IActionResult> CreateAndPushNotificationToUserByDeviceToken([FromBody] SendUserNotificationDto notification)
    {
        if (string.IsNullOrEmpty(notification.DeviceToken))
            return ToBadRequest("Device token is required.");

        try
        {
            await _notificationHelper.PublishNotificationToUserAsync(notification);
            return ToSuccess("Notification pushed.");
        }
        catch (Exception ex)
        {
            // Log if needed
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpPost("push-to-topic")]
    public async Task<IActionResult> CreateAndPushNotificationToTopic([FromBody] SendTopicNotificationDto notification)
    {
        if (string.IsNullOrEmpty(notification.Topic))
            return ToBadRequest("Topic is required.");

        try
        {
            await _notificationHelper.PublishNotificationToTopicAsync(notification);
            return ToSuccess("Notification pushed.");
        }
        catch (Exception ex)
        {
            // Log if needed
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
}