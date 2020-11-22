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
        
        private Dictionary<ResourceId, Item> _itemDic = new Dictionary<ResourceId, Item>();

        public void Initialize() {
            foreach (var item in _items) {
                _itemDic.Add(item.Id, item);
            }
        }

        public Item GetItemById(ResourceId id) {
            return _itemDic[id];
        }
    }
}