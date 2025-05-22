using AutoMapper;
using Firebase_Auth.Data.Entities.Common.Notification;
using Firebase_Auth.Data.Models.Common.Notification;

namespace Firebase_Auth.Data.Profiles;
public class NotificationTopicProfile : Profile
{
    public NotificationTopicProfile()
    {
        CreateMap<NotificationTopic, NotificationTopicDto>().ReverseMap();
        CreateMap<NotificationTopic, CreateNotificationTopicDto>().ReverseMap();
    }
}