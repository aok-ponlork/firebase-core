using Firebase_Auth.Common;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Firebase.FCM;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Common.Notification;
[Route("api/notification")]
[Authorize]
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
    public async Task<IActionResult> ListGeneralNotificationAsync([FromQuery] PaginationFilter filter)
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
    [Authorize]
    [HttpPost("/general-notification")]
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
    [AllowAnonymous]
    [HttpPost("/push-user-notificaiton")]
    public async Task<IActionResult> CreateAndPushNotificationToUserByDeviceToken([FromBody] NotificationDto notification)
    {
        try
        {
            //await _notificationHelper.SendUserNotificationAsync(notification);
            return ToSuccess("Notificaiton Pushed.");
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
}