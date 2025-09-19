using Firebase_Auth.Common.Filters;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Response;
namespace Firebase_Auth.Services.Interfaces;
public interface INotificationService
{
    Task<CreateUserNotificationDto> CreateNotificationForSpecificUserAsync(CreateUserNotificationDto notification);
    Task<CreateGeneralNotificationDto> CreateGeneralNotificationAsync(CreateGeneralNotificationDto notification);
    Task DeleteNotificationAsync(Guid Id);
    Task<PaginationResponse<NotificationListDto>> GetGeneralNotificationForClientPagination(SimpleFilter  notificationFilter);
    Task<PaginationResponse<NotificationListDto>> GetGeneralNotificationPagination(SimpleFilter  notificationFilter);
    Task<PaginationResponse<NotificationListDto>> GetUserNotificationPagination(SimpleFilter  notificationFilter);
    Task<NotificationDto?> FindNotificationByIdAsync(Guid id);
    Task<NotificationDto> UpdateNotificationAsync(NotificationDto notification);
    Task CreateNotificationsForMultipleUsersAsync(CreateUserNotificationDto notification, List<string> userIds);
    Task ResendNotificationAsync(Guid id);
    Task<string?> GetUserDeviceTokenByUserIdAsync(Guid id);
    Task PermanentDeleteNotificationAsync(List<Guid> Ids);
}