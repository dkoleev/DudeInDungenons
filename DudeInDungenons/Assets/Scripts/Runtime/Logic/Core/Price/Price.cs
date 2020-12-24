using System;
using System.Collections.Generic;
using Runtime.Data.Items;
using UnityEngine;

namespace Runtime.Logic.Core.Price {
    [Serializable]
    public class Price {
        [SerializeField]
        private List<ItemStack> _value;
        
        public List<ItemStack> Value => _value;

        public bool Check(Inventory.Inventory inventory) {
            return inventory.Have(_value);
        }
    }
}