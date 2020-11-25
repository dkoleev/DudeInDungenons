using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [Serializable]
    public class PLayerStartInventoryItemStack: ItemStack {
        [SerializeField, TableColumnWidth(50, false), PropertyOrder(4)]
        private bool _equipped;
        
        public bool Equipped => _equipped;
    }
}