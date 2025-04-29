using Firebase_Auth.Common;
using Firebase_Auth.Data.Models.Common.Notification;
namespace Firebase_Auth.Services.Interfaces;
public interface INotificationService
{
    Task<CreateNotificationDto> CreateNotificationForSpecificUserAsync(CreateNotificationDto notification);
    Task<CreateNotificationDto> CreateGeneralNotificationAsync(CreateNotificationDto notification);
    Task DeleteNotificationAsync(Guid Id);
    Task<PaginationResponse<NotificationListDto>> GetGeneralNotificationForClientPagination(PaginationFilter notificationFilter);
    Task<PaginationResponse<NotificationListDto>> GetGeneralNotificationPagination(PaginationFilter notificationFilter);
    Task<PaginationResponse<NotificationListDto>> GetUserNotificationPagination(PaginationFilter notificationFilter);
    Task<NotificationDto?> FindNotificationByIdAsync(Guid id);
    Task<int> UpdateNotificationAsync(NotificationDto notification);
    Task CreateNotificationsForMultipleUsersAsync(CreateNotificationDto notification, List<string> userIds);
    Task ResendNotificationAsync(Guid id);
    Task<string?> GetUserDeviceTokenByIdAsync(Guid id);
}