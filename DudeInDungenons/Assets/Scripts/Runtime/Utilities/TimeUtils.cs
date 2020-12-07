using System;
using UnityEngine;

namespace Runtime.Utilities {
    public static class TimeUtils {
        public enum TimeCaptionDetails {
            HHMMSS,
            MMSS,
            SS
        }
        
        public static long Current => (long)(DateTime.UtcNow - EpochStart).TotalMilliseconds;
        
        private static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static string GetTimeCaption(float milliseconds, TimeCaptionDetails details = TimeCaptionDetails.HHMMSS) {
            if (milliseconds > TimeSpan.MaxValue.TotalMilliseconds) {
                Debug.LogError($"value {milliseconds} must be less then {TimeSpan.MaxValue.TotalMilliseconds}");
                return string.Empty;
            }

            if (milliseconds < 0) {
                milliseconds = 0;
            }

            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
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