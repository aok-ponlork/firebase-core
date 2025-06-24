using Firebase_Auth.Common.Filters;
using Firebase_Auth.Data.Entities.Common.Notification;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Response;
using Firebase_Auth.Services.Interfaces.Base;

namespace Firebase_Auth.Services.Interfaces;
public interface INotificationTopicService : IBaseService<CreateNotificationTopicDto, NotificationTopic, NotificationTopicDto>
{ 
    Task<PaginationResponse<NotificationTopicDto>> List(FilterRequest filter);
}
