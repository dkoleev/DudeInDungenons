using System;
using Newtonsoft.Json;

namespace Runtime.Logic.GameProgress.Progress.Items {
    public class ItemTimeProgress : ItemProgress {
        [JsonProperty("Timer")]
        private TimerProgress _timer;
        
        [JsonIgnore]
        public TimerProgress Timer => _timer;

        public ItemTimeProgress() {
            _timer = new TimerProgress();
        }

        public ItemTimeProgress(string id, int amount, long targetTime) : base(id, amount) {
            _timer = new TimerProgress();
            _timer.SetTarget(targetTime);
        }
        
        public void SetTimerTarget(long time) {
            _timer.SetTarget(time);
        }

        public override void Spend(int amount, long restoreTime) {
            base.Spend(amount, restoreTime);
            
            SetTimerTarget(restoreTime);
        }
    }
}