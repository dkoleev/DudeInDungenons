using System.Collections.Generic;

namespace Runtime.Logic.Inventory {
    public class Inventory {
        private Dictionary<ResourceId, int> _inventory;
        
        public Inventory(GameProgress.GameProgress gameProgress) {
            _inventory = gameProgress.Player.Inventory;
        }

        public IReadOnlyDictionary<ResourceId, int> Get() {
            return _inventory;
        }

        public void AddResource(ResourceId id, int amount) {
            if (_inventory.ContainsKey(id)) {
                _inventory[id] += amount;
            } else {
                _inventory.Add(id, amount);
            }
        }
        
        public void AddResource(KeyValuePair<ResourceId, int> item) {
            AddResource(item.Key, item.Value);
        }

        public void Add(Dictionary<ResourceId, int> drop) {
            foreach (var dropItem in drop) {
                AddResource(dropItem);
            }
        }

        public InventoryOperationResult SpendResource(ResourceId id, int amount) {
            if (!_inventory.ContainsKey(id) || _inventory[id] < amount) {
                return InventoryOperationResult.NoEnoughResource;
            }

            _inventory[id] -= amount;

            return InventoryOperationResult.Success;
        }

        public int GetResourceAmount(ResourceId resourceId) {
            if (!_inventory.ContainsKey(resourceId)) {
                return 0;
            }

            return _inventory[resourceId];
        }
        
        public enum InventoryOperationResult {
            Success,
            NoEnoughResource
        }
    }
}