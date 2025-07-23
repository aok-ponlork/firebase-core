using Firebase_Auth.Data.Entities.Common.Notification;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Data.Seed
{
    public static class NotificationTopicSeeder
    {
        public static void SeedNotificationTopics(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTopic>().HasData(
                new NotificationTopic
                {
                    Id = Guid.NewGuid(),
                    TopicName = "general-notification",
                    Description = "General notifications for all users"
                },
                new NotificationTopic
                {
                    Id = Guid.NewGuid(),
                    TopicName = "sports-news",
                    Description = "Latest sports news and updates"
                },
                new NotificationTopic
                {
                    Id = Guid.NewGuid(),
                    TopicName = "weather-updates",
                    Description = "Weather alerts and updates"
                },
                new NotificationTopic
                {
                    Id = Guid.NewGuid(),
                    TopicName = "marketing-promo",
                    Description = "Special offers and marketing promotions"
                }
            );
        }
    }
}
