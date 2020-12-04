using System;
using UnityEngine;

namespace Runtime.Utilities {
    public static class TimeUtils {
        public enum TimeCaptionDetails {
            HHMMSS,
            MMSS,
            SS
        }
        
        public static long Current => (long)(DateTime.UtcNow - EpochStart).TotalSeconds;
        
        private static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static string GetTimeCaption(float seconds, TimeCaptionDetails details = TimeCaptionDetails.HHMMSS) {
            if (seconds > TimeSpan.MaxValue.TotalSeconds) {
                Debug.LogError($"value {seconds} must be less then {TimeSpan.MaxValue.TotalSeconds}");
                return string.Empty;
            }

            if (seconds < 0) {
                seconds = 0;
            }

            var timeSpan = TimeSpan.FromSeconds(seconds);
            switch (details) {
                case TimeCaptionDetails.HHMMSS:
                    return timeSpan.ToString(@"hh\:mm\:ss");
                case TimeCaptionDetails.MMSS:
                    return timeSpan.ToString(@"mm\:ss");
                case TimeCaptionDetails.SS:
                    return timeSpan.ToString(@"ss");
                default:
                    Debug.LogError("no details");
                    return String.Empty;
            }
        }
    }
}