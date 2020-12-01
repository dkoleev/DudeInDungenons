using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;

namespace Runtime.Logic {
    public class ResourceConverter : IEventReceiver<OnAddResourceToInventory>, IDisposable {
        private Item _from;
        private Item _to;
        private Dictionary<int, int> _convertModel;
        private Inventory.Inventory _inventory;
        
        public ResourceConverter(Item from, Item to, Dictionary<int, int> convertModel, Inventory.Inventory inventory) {
            _from = from;
            _to = to;
            _convertModel = convertModel;
            _inventory = inventory;
            
            EventBus.Register(this);
        }

        public void OnEvent(OnAddResourceToInventory e) {
            if (e.ResourceId != _from.Id) {
                return;
            }

            Convert();
        }

        private void Convert() {
            var newFromAmount = _inventory.GetResourceAmount(_from.Id);
            var curToAmount = _inventory.GetResourceAmount(_to.Id);
            if (curToAmount == _convertModel.Last().Key) {
                return;
            }

            var needToConvert = _convertModel[curToAmount + 1];
            if (newFromAmount >= needToConvert) {
                _inventory.Add(_to.Id, 1);
                Convert();
            }
        }

        public void Dispose() {
            EventBus.UnRegister(this);
        }
    }
}
