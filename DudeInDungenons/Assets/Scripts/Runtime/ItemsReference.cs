using System.Collections.Generic;
using Runtime.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class ItemsReference : MonoBehaviour {
        [SerializeField, Required]
        [InlineEditor]
        private List<Item> _items;
        
        private Dictionary<string, Item> _itemDic = new Dictionary<string, Item>();

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