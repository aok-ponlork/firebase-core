using AutoMapper;
using Firebase_Auth.Data.Entities.Common.Notification;
using Firebase_Auth.Data.Models.Common.Notification;

namespace Firebase_Auth.Data.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        // From entity to DTO
        CreateMap<Notification, NotificationDto>();
        CreateMap<Notification, NotificationListDto>();

        // From DTO to entity (for creating and updating)
        CreateMap<NotificationDto, Notification>();
        CreateMap<CreateUserNotificationDto, Notification>();
        CreateMap<CreateGeneralNotificationDto, Notification>();
    }
}
