using Runtime.Data.Items;
using Runtime.Logic.GameProgress.Progress.Items;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Logic.Converters {
    public class TimeToResourceConverter : IResourceConverter {
        private ItemRestoreByTime _item;
        private Inventory.Inventory _inventory;
        private ItemTimeProgress _itemProgress;
        private long _totalMilliseconds;

        private bool _initialized;

        public TimeToResourceConverter(ItemRestoreByTime item, Inventory.Inventory inventory) {
            _item = item;
            _inventory = inventory;
            _itemProgress = _inventory.GetItem(_item.Id) as ItemTimeProgress;
            
            _totalMilliseconds = _item.RestoreTime.TotalMilliseconds;
        }

        public void Update() {
            if (!_initialized) {
                return;
            }

            if (_itemProgress.Amount >= _item.MaxAmount) {
                return;
            }

            if (_itemProgress.Timer.Remaining <= 0) {
                _itemProgress.SetTimerTarget(_totalMilliseconds);
                _inventory.Add(_item.Id, 1);
            }
        }

        public void ApplyGameOutProgress(GameProgress.GameProgress progress) {
            var timeElapsedSinceStart = TimeUtils.Current - _itemProgress.Timer.StartTime;
            
            var amountItemsToAdd = timeElapsedSinceStart /_totalMilliseconds;
            var timeLeftAfterRestore = _totalMilliseconds - timeElapsedSinceStart % _totalMilliseconds;
            _itemProgress.SetTimerTarget(timeLeftAfterRestore);

            amountItemsToAdd = Mathf.Min((int)amountItemsToAdd, _item.MaxAmount - _itemProgress.Amount);

            _inventory.Add(_item.Id, (int)amountItemsToAdd);

            _initialized = true;
        }
    }
}