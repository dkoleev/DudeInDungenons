using System;

namespace Runtime.LocalNotifications {
    public class LocalNotificationsRegistration {
        public const string ChannelId = "game_channel0";
        public const string ReminderChannelId = "reminder_channel1";
        public const string NewsChannelId = "news_channel2";

        private GameNotificationsManager _manager;
        
        public LocalNotificationsRegistration(GameNotificationsManager manager) {
            _manager = manager;
            
            var c1 = new GameNotificationChannel(ChannelId, "Default Game Channel", "Generic notifications");
            var c2 = new GameNotificationChannel(NewsChannelId, "News Channel", "News feed notifications");
            var c3 = new GameNotificationChannel(ReminderChannelId, "Reminder Channel", "Reminder notifications");
            
            _manager.Initialize(c1, c2);
        }
        
        public void SendNotification(string title, string body, DateTime deliveryTime, int? badgeNumber = null,
            bool reschedule = false, string channelId = null,
            string smallIcon = null, string largeIcon = null)
        {
            IGameNotification notification = _manager.CreateNotification();

            if (notification == null)
            {
                return;
            }

            notification.Title = title;
            notification.Body = body;
            notification.Group = !string.IsNullOrEmpty(channelId) ? channelId : ChannelId;
            notification.DeliveryTime = deliveryTime;
            notification.SmallIcon = smallIcon;
            notification.LargeIcon = largeIcon;
            if (badgeNumber != null)
            {
                notification.BadgeNumber = badgeNumber;
            }

            PendingNotification notificationToDisplay = _manager.ScheduleNotification(notification);
            notificationToDisplay.Reschedule = reschedule;

          //  QueueEvent($"Queued event with ID \"{notification.Id}\" at time {deliveryTime:HH:mm}");
        }
    }
}