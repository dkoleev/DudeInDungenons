using System;
using System.Collections.Generic;
using Runtime.Data.Items;
using Runtime.Logic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class ItemsReference : MonoBehaviour {
        [SerializeField, Required]
        [InlineEditor]
        private List<Item> _items;

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
        public Item ExpData => _expData;
        public Item GemData => _gemData;
        public Item GoldData => _goldData;
        public Item LevelData => _levelData;
        public Item EnergyData => _energyData;

        public void Initialize() {
            foreach (var item in _items) {
                _itemDic.Add(item.Id, item);
            }
        }

        public Item GetItemById(string id) {
            return _itemDic[id];
        }
    }
}