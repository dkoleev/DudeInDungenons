using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Logic.Core.BaseTypes {
    [Serializable]
    public class TargetTime {
        [MinValue(0), SerializeField]
        private int _days = 0;
        [MinValue(0), SerializeField]
        private int _hours = 0;
        [MinValue(0), SerializeField]
        private int _minutes = 0;
        [MinValue(0), SerializeField]
        private int _seconds = 0;

        public long TotalMilliseconds => GetInMilliseconds();
        public long TotalSeconds { get; private set; }

        private long GetInMilliseconds() {
            var span = new TimeSpan(_days, _hours, _minutes, _seconds);
            
            return (long) Math.Floor(span.TotalMilliseconds);
        }
        
        private long GetInSeconds() {
            var span = new TimeSpan(_days, _hours, _minutes, _seconds);
            
            return (long) Math.Floor(span.TotalSeconds);
        }
    }
}