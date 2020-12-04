using Runtime.Data.Items;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Utilities;

namespace Runtime.Logic.Converters {
    public class TimeToResourceConverter : IResourceConverter {
        private ItemRestoreByTime _item;
        private Inventory.Inventory _inventory;
        private ItemProgress _itemProgress;
        
        public TimeToResourceConverter(ItemRestoreByTime item, Inventory.Inventory inventory) {
            _item = item;
            _inventory = inventory;
            _itemProgress = _inventory.GetItem(_item.Id);
        }

        public void Update() {
            if (_itemProgress.Amount >= _item.MaxAmount) {
                return;
            }

            if (_itemProgress.TimeLeft <= 0) {
                _itemProgress.SetTime(_item.RestoreTime);
            }

            if (TimeUtils.Current >= _itemProgress.TimeLeft) {
                _itemProgress.SetTime(_item.RestoreTime);
                _inventory.Add(_item.Id, 1);
            }
        }
    }
}