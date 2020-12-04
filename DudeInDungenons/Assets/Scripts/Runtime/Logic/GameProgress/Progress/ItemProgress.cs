using Newtonsoft.Json;
using Runtime.Utilities;

namespace Runtime.Logic.GameProgress.Progress {
    public class ItemProgress {
        [JsonProperty]
        private string _id;
        [JsonProperty]
        private int _amount;
        [JsonProperty]
        private long _timeLeft;

        [JsonIgnore]
        public string Id => _id;
        [JsonIgnore]
        public int Amount => _amount;
        [JsonIgnore]
        public long TimeLeft => _timeLeft;

        public ItemProgress() {
            
        }

        public ItemProgress(string id, int amount) {
            _id = id;
            _amount = amount;
            _timeLeft = 0;
        }

        public ItemProgress(string id, int amount, long timeLeft) {
            _id = id;
            _amount = amount;
            _timeLeft = timeLeft;
        }

        public void Add(int amount) {
            _amount += amount;
        }

        public void Remove(int amount) {
            if (_amount < amount) {
                _amount = 0;
            } else {
                _amount -= amount;
            }
        }
        
        public void Spend(int amount, long restoreTime) {
            Remove(amount);
            SetTime(restoreTime);
        }

        public void SetTime(long time) {
            _timeLeft =  TimeUtils.Current + time;
        }
    }
}