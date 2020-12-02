namespace Runtime.LocalNotifications {
    public class LocalNotificationsRegistration {
        public const string ChannelId = "game_channel0";
        public const string ReminderChannelId = "reminder_channel1";
        public const string NewsChannelId = "news_channel2";
        
        public LocalNotificationsRegistration(GameNotificationsManager manager) {
            var c1 = new GameNotificationChannel(ChannelId, "Default Game Channel", "Generic notifications");
            var c2 = new GameNotificationChannel(NewsChannelId, "News Channel", "News feed notifications");
            var c3 = new GameNotificationChannel(ReminderChannelId, "Reminder Channel", "Reminder notifications");
            
            manager.Initialize(c1, c2);
        }
    }
}