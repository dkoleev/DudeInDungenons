using System;
using Runtime.LocalNotifications;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using UnityEngine;

namespace Runtime.Logic.Managers {
    public class LocalNotificationManager : ManagerBase, IEventReceiver<OnApplicationQuit> {
        public const string ChannelId = "game_channel0";
        public const string ReminderChannelId = "reminder_channel1";
        public const string NewsChannelId = "news_channel2";

        private GameNotificationsManager _gameNotificationsManager;
        
        public LocalNotificationManager(GameController gameController) : base(gameController) {
            EventBus.Register(this);
            
            _gameNotificationsManager = gameController.LocalNotificationsManager;
            
            var c1 = new GameNotificationChannel(ChannelId, "Default Game Channel", "Generic notifications");
            var c2 = new GameNotificationChannel(NewsChannelId, "News Channel", "News feed notifications");
            var c3 = new GameNotificationChannel(ReminderChannelId, "Reminder Channel", "Reminder notifications");
            
            _gameNotificationsManager.Initialize(c1, c2);
        }
        public override void Update() {
            
        }

        public void SendNotification(string title,
            string body,
            long delayInMilliseconds,
            int? badgeNumber = null,
            bool reschedule = false,
            string channelId = null,
            string smallIcon = null,
            string largeIcon = null) {
            
            var triggerDate = DateTime.Now.ToLocalTime() + TimeSpan.FromMilliseconds(delayInMilliseconds);
            SendNotification(title, body, triggerDate, badgeNumber, reschedule, channelId, smallIcon, largeIcon);
        }

        public void SendNotification(string title, string body, DateTime deliveryTime, int? badgeNumber = null,
            bool reschedule = false, string channelId = null,
            string smallIcon = null, string largeIcon = null)
        {
            IGameNotification notification = _gameNotificationsManager.CreateNotification();

            if (notification == null)
            {
                return;
            }

            notification.Title = title;
            notification.Body = body;
            notification.Group = !string.IsNullOrEmpty(channelId) ? channelId : ChannelId;
            notification.DeliveryTime = deliveryTime;
            notification.SmallIcon = "icon_0";
            notification.LargeIcon = "icon_1";
            notification.ShouldAutoCancel = true;
            if (badgeNumber != null)
            {
                notification.BadgeNumber = badgeNumber;
            }

            PendingNotification notificationToDisplay = _gameNotificationsManager.ScheduleNotification(notification);
            notificationToDisplay.Reschedule = reschedule;

            Debug.Log($"Queued local notification with Id {notification.Id} at time {deliveryTime:g}; Title: {notification.Title}");
        }
        
                
        private int playReminderHour = 6;
        public void OnPlayReminder()
        {
            // Schedule a reminder to play the game. Schedule it for the next day.
            DateTime deliveryTime = DateTime.Now.ToLocalTime().AddDays(1);
            deliveryTime = new DateTime(deliveryTime.Year, deliveryTime.Month, deliveryTime.Day, playReminderHour, 0, 0,
                DateTimeKind.Local);

            SendNotification("Cookie Reminder", "Remember to make more cookies!", deliveryTime,
                channelId: ReminderChannelId);
        }
        
        public void OnEvent(OnApplicationQuit e) {
            RegisterNotificationOnGameExit();
        }

        private void RegisterNotificationOnGameExit() {
            var energyData = GameController.ItemReference.EnergyData;
            var currentEnergy =
                GameController.Progress.Player.GetInventoryItem(energyData.Id);
            if (currentEnergy != null && currentEnergy.Amount < energyData.MaxAmount) {
                var amountToRestore = energyData.MaxAmount - currentEnergy.Amount;
                if (amountToRestore < 3) {
                    return;
                }

                var energyNotifyTime = amountToRestore * energyData.RestoreTime.TotalMilliseconds;
                SendNotification("Energy Full", "Adventure time!", energyNotifyTime);
            }
        }

        public override void Dispose() {
            EventBus.UnRegister(this);
        }
    }
}