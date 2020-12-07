using System;
using Newtonsoft.Json;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Logic.GameProgress.Progress {
    [Serializable]
    public class TimerProgress {
        [JsonProperty("TargetTime")]
        private long _targetTime;
        [JsonProperty("StartTime")]
        private long _startTime;

        public TimerProgress() {
            _targetTime = TimeUtils.Current;
        }

        [JsonIgnore]
        public long TargetTime => _targetTime;
        [JsonIgnore]
        public long StartTime => _startTime;

        [JsonIgnore]
        public long Elapsed => TimeUtils.Current - _startTime;
        [JsonIgnore]
        public long Remaining {
            get {
                var remaining = _targetTime - TimeUtils.Current;
                if (remaining < 0) {
                    remaining = 0;
                }

                return remaining;
            }
        }

        public void SetTarget(long time) {
            _startTime = TimeUtils.Current;
            _targetTime = _startTime + time;
        }
    }
}