using System.Collections.Generic;
using Runtime.Logic.GameProgress;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.MainMenu {
    public class Inventory : UiBase {
        [SerializeField, AssetsOnly, Required]
        private InventoryItem _itemPrefab;
        [SerializeField, Required]
        private GridLayoutGroup _inventoryGrid;

        private List<InventoryItem> _items;

        public override void Initialize(GameProgress progress, ItemsReference itemsReference) {
            base.Initialize(progress, itemsReference);
            
            _items = new List<InventoryItem>();
            foreach (var item in Progress.Player.Inventory) {
                var itemData = ItemsReference.GetItemById(item.Key);
                var inventoryItem = Instantiate(_itemPrefab, _inventoryGrid.transform);
                inventoryItem.Initialize(itemData.Icon, item.Value.ToString());
                _items.Add(inventoryItem);
            }
        }
    }
}