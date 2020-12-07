using System.Collections.Generic;
using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Logic.GameProgress.Progress.Items;

namespace Runtime.Logic.Inventory {
    public class Inventory {
        private Dictionary<string, ItemProgress> _inventory;
        
        public Inventory(GameProgress.GameProgress gameProgress) {
            _inventory = gameProgress.Player.Inventory;
        }

        public IReadOnlyDictionary<string, ItemProgress> Get() {
            return _inventory;
        }
        
        public ItemProgress GetItem(string id) {
            if (!_inventory.ContainsKey(id)) {
                return null;
            }

            return _inventory[id];
        }

        public void Add(string id, int amount) {
            if (amount <= 0) {
                return;
            }

            if (_inventory.ContainsKey(id)) {
                _inventory[id].Add(amount);
            } else {
                _inventory.Add(id, new ItemProgress(id, amount));
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
                _inventory[drop.Item.Id].Add(drop.Amount);
            } else {
                _inventory.Add(drop.Item.Id, new ItemProgress(drop.Item.Id, drop.Amount));
            }
            
            EventBus<OnAddResourceToInventory>.Raise(new OnAddResourceToInventory(drop.Item.Id, drop.Amount));
        }

        public bool Have(Item item, int amount) {
            return _inventory.ContainsKey(item.Id) && _inventory[item.Id].Amount >= amount;
        }
        
        public bool Have(ItemStack item) {
            return Have(item.Item, item.Amount);
        }
        
        public bool Have(List<ItemStack> items) {
            foreach (var item in items) {
                if (!Have(item)) {
                    return false;
                }
            }
           
            return true;
        }
        
        public InventoryOperationResult SpendResource(Item item, int amount) {
            if (!Have(item, amount)) {
                return InventoryOperationResult.NoEnoughResource;
            }

            if (item is ItemRestoreByTime itemRestoreByTime) {
                _inventory[item.Id].Spend(amount, itemRestoreByTime.RestoreTime.TotalMilliseconds);
            } else {
                _inventory[item.Id].Remove(amount);
            }

            
            EventBus<OnSpendResources>.Raise(new OnSpendResources(item, amount));

            return InventoryOperationResult.Success;
        }

        public InventoryOperationResult SpendResource(ItemStack item) {
            return SpendResource(item.Item, item.Amount);
        }
        
        public InventoryOperationResult SpendResource(List<ItemStack> items) {
            foreach (var item in items) {
                if (!Have(item.Item, item.Amount)) {
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

            return _inventory[resourceId].Amount;
        }
        
        public enum InventoryOperationResult {
            Success,
            NoEnoughResource
        }
    }
}