using System;
using System.Collections.Generic;
using Runtime.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Logic.Core { 
    [Serializable]
    public class Action {
        [Title("Price")]
        [SerializeField]
        private bool _havePrice;
        [SerializeField, ShowIf("_havePrice")]
        private List<ItemStack> _price;
        [Title("Reward")]
        [SerializeField]
        private bool _haveReward;
        [SerializeField, ShowIf("_haveReward")]
        private List<ItemStack> _reward;
        [Title("Requirement")]
        [SerializeField]
        private bool _haveRequirement;
        [SerializeField, ShowIf("_haveRequirement")]
        private List<ItemStack> _requirement;

        public List<ItemStack> Price => _price;
        public List<ItemStack> Reward => _reward;
        public List<ItemStack> Requirement => _requirement;

        public bool Award(Inventory.Inventory inventory) {
            if (!Have(_reward, _haveReward)) {
                return true;
            }
            
            inventory.Add(_reward);

            return true;
        }

        public bool Pay(Inventory.Inventory inventory) {
            if (!Have(_price, _havePrice)) {
                return true;
            }
            
            return inventory.SpendResource(_price) == Inventory.Inventory.InventoryOperationResult.Success;
        }

        public bool Check(Inventory.Inventory inventory) {
            if (!Have(_requirement, _haveRequirement)) {
                return true;
            }

            return inventory.Have(_requirement);
        }

        private bool Have(List<ItemStack> target, bool have) {
            if (!have || target is null || target.Count == 0) {
                return false;
            }

            return true;
        }
    }
}