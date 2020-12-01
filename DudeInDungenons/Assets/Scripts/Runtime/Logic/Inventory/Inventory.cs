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

        public void Add(string id, int amount) {
            if (_inventory.ContainsKey(id)) {
                _inventory[id] += amount;
            } else {
                _inventory.Add(id, amount);
            }
            
            EventBus<OnAddResourceToInventory>.Raise(new OnAddResourceToInventory(id, amount));
        }
        
        public void Add(KeyValuePair<string, int> item) {
            Add(item.Key, item.Value);
        }

        public void Add(Dictionary<string, int> drop) {
            foreach (var dropItem in drop) {
                Add(dropItem);
            }
        }
        
        public void Add(List<ItemStack> drop) {
            foreach (var dropItem in drop) {
                Add(dropItem);
            }
        }
        
        public void Add(ItemStack drop) {
            if (_inventory.ContainsKey(drop.Item.Id)) {
                _inventory[drop.Item.Id] += drop.Amount;
            } else {
                _inventory.Add(drop.Item.Id, drop.Amount);
            }
            
            EventBus<OnAddResourceToInventory>.Raise(new OnAddResourceToInventory(drop.Item.Id, drop.Amount));
        }

        public bool Have(ItemStack item) {
            return _inventory.ContainsKey(item.Item.Id) && _inventory[item.Item.Id] >= item.Amount;
        }
        
        public bool Have(List<ItemStack> items) {
            foreach (var item in items) {
                if (!Have(item)) {
                    return false;
                }
            }
           
            return true;
        }

        public InventoryOperationResult SpendResource(ItemStack item) {
            if (!Have(item)) {
                return InventoryOperationResult.NoEnoughResource;
            }

            _inventory[item.Item.Id] -= item.Amount;
            
            EventBus<OnSpendResources>.Raise(new OnSpendResources(item));

            return InventoryOperationResult.Success;
        }
        
        public InventoryOperationResult SpendResource(List<ItemStack> items) {
            foreach (var item in items) {
                if (!Have(item)) {
                    return InventoryOperationResult.NoEnoughResource;
                }
            }

            foreach (var item in items) {
                SpendResource(item);
            }

            return InventoryOperationResult.Success;
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