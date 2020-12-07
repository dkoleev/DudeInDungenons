using Newtonsoft.Json;

namespace Runtime.Logic.GameProgress.Progress.Items {
    public class ItemProgress {
        [JsonProperty("Id")]
        private string _id;
        [JsonProperty("Amount")]
        private int _amount;

        [JsonIgnore]
        public string Id => _id;
        [JsonIgnore]
        public int Amount => _amount;
     
        public ItemProgress() { }

        public ItemProgress(string id, int amount) {
            _id = id;
            _amount = amount;
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
        
        public virtual void Spend(int amount, long restoreTime) {
            Remove(amount);
        }
    }
}