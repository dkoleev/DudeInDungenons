using System.Collections.Generic;
using Runtime.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class ItemsReference : SerializedMonoBehaviour {
        public enum ItemType {
            Resources,
            Pets,
            Weapons,
            PlayerSkins
        }
        
        [SerializeField, Required]
        private Dictionary<ItemType, List<Item>> _itemsByType;

        [SerializeField, Required, InlineEditor]
        private Item _gemData;
        [SerializeField, Required, InlineEditor]
        private Item _goldData;
        [SerializeField, Required, InlineEditor]
        private Item _expData;
        [SerializeField, Required, InlineEditor]
        private Item _levelData;
        [SerializeField, Required, InlineEditor]
        private Item _energyData;
        
        private Dictionary<string, Item> _itemDic = new Dictionary<string, Item>();
        private Dictionary<ItemType, Dictionary<string, Item>> _itemDicByType = new Dictionary<ItemType, Dictionary<string, Item>>();
        public Item ExpData => _expData;
        public Item GemData => _gemData;
        public Item GoldData => _goldData;
        public Item LevelData => _levelData;
        public Item EnergyData => _energyData;

        public void Initialize() {
            foreach (var item in _itemsByType) {
                if (!_itemDicByType.ContainsKey(item.Key)) {
                    _itemDicByType.Add(item.Key, new Dictionary<string, Item>());
                }

                foreach (var item1 in _itemsByType[item.Key]) {
                    _itemDicByType[item.Key].Add(item1.Id, item1);
                    _itemDic.Add(item1.Id, item1);
                }
            }
        }
        
        public List<Item> GetItems(ItemType type) {
            return _itemsByType[type];
        }

        public Item GetItem(ItemType type, string id) {
            return _itemDicByType[type][id];
        }
        
        public Item GetItem(string id) {
            return _itemDic[id];
        }
    }
}