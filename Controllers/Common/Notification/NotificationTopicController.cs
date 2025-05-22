using Firebase_Auth.Common;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Firebase.FCM;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Firebase_Auth.Controllers.Common.Notification;
[Route("api/notificaiton-topic")]
[Authorize]
public class NotificationTopicController : CoreController
{
    private readonly INotificationTopicService _service;
    private readonly NotificationHelper _notificationHelper;

    public NotificationTopicController(INotificationTopicService service, NotificationHelper notificationHelper, ILogger<CoreController> logger) : base(logger)
    {
        _service = service;
        _notificationHelper = notificationHelper;
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateNotificationTopicDto payload)
    {
        try
        {
            var result = await _service.AddAsync(payload);
            return ToSuccess("Topic created!", result);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");

        }
    }
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] PaginationFilter filter)
    {
        try
        {
            var response = await _service.List(filter);
            return ToSuccess("Success! ", response);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromQuery] string Id, [FromBody] CreateNotificationTopicDto dto)
    {
        try
        {
            if (string.IsNullOrEmpty(Id))
            {
                return ToBadRequest("ID can't be empty or null");
            }
            var result = await _service.UpdateAsync(Guid.Parse(Id), dto);
            return ToSuccess("Update success!", result);
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var result = await _service.GetByIdAsync(Guid.Parse(id));
            return ToSuccess("Success!", result);
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            await _service.DeleteAsync(Guid.Parse(id));
            return ToNoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
}