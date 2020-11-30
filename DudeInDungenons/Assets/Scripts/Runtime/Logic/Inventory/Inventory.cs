using System.Collections.Generic;
using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;

namespace Runtime.Logic.Inventory {
    public class Inventory {
        private Dictionary<string, int> _inventory;
        
        public Inventory(GameProgress.GameProgress gameProgress) {
            _inventory = gameProgress.Player.Inventory;
        }

        public IReadOnlyDictionary<string, int> Get() {
            return _inventory;
        }

        public void AddResource(string id, int amount) {
            if (_inventory.ContainsKey(id)) {
                _inventory[id] += amount;
            } else {
                _inventory.Add(id, amount);
            }
            
            EventBus<OnAddResourceToInventory>.Raise(new OnAddResourceToInventory(id, amount));
        }
        
        public void AddResource(KeyValuePair<string, int> item) {
            AddResource(item.Key, item.Value);
        }

        public void Add(Dictionary<string, int> drop) {
            foreach (var dropItem in drop) {
                AddResource(dropItem);
            }
        }

        public InventoryOperationResult SpendResource(string id, int amount) {
            if (!_inventory.ContainsKey(id) || _inventory[id] < amount) {
                return InventoryOperationResult.NoEnoughResource;
            }

            _inventory[id] -= amount;
            
            EventBus<OnSpendResources>.Raise(new OnSpendResources(id, amount));

            return InventoryOperationResult.Success;
        }
        
        public InventoryOperationResult SpendResource(ItemStack item) {
            return SpendResource(item.Item.Id, item.Amount);
        }

        public int GetResourceAmount(string resourceId) {
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