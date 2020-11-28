using System.Collections.Generic;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class Inventory : UiBase {
        [SerializeField, AssetsOnly, Required]
        private InventoryItem _itemPrefab;
        [SerializeField, Required]
        private GridLayoutGroup _inventoryGrid;

        private List<InventoryItem> _items;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
            _items = new List<InventoryItem>();
            foreach (var item in GameController.Inventory.Get()) {
                var itemData = ItemsReference.GetItemById(item.Key);
                var inventoryItem = Instantiate(_itemPrefab, _inventoryGrid.transform);
                inventoryItem.Initialize(itemData.Icon, item.Value.ToString());
                _items.Add(inventoryItem);
            }
        }
    }
}