using Firebase_Auth.Common;
using Firebase_Auth.Data.Entities.Common.Notification;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Services.Interfaces.Base;

namespace Firebase_Auth.Services.Interfaces;
public interface INotificationTopicService : IBaseService<CreateNotificationTopicDto, NotificationTopic, NotificationTopicDto>
{ 
    Task<PaginationResponse<NotificationTopicDto>> List(PaginationFilter filter);
}
