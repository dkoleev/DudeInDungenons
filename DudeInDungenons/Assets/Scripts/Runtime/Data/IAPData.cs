using System;
using System.Collections.Generic;
using Runtime.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "IAP", menuName = "Data/IAP")]
    public class IAPData : SerializedScriptableObject {
        [Title("Key - id product in store, Value - reward")]
        [SerializeField]
        [TableList]
        private List<IAPItemDataStore> _storeItems;
        [Title("Key - id price resource, Value - reward resource")]
        [SerializeField]
        [TableList]
        private List<IAPItemDataResource> _resourceItems;

        public List<IAPItemDataStore> StoreItems => _storeItems;
        public List<IAPItemDataResource> ResourceItems => _resourceItems;
    }

    [Serializable]
    public class IAPItemDataResource {
        [SerializeField]
        private string _id;
        [SerializeField]
        private string _title;
        [SerializeField]
        private ItemStack _price;
        [SerializeField]
        private ItemStack _reward;

        public ItemStack Price => _price;
        public ItemStack Reward => _reward;
        public string Title => _title;
        public string Id => _id;
    }
    
    [Serializable]
    public class IAPItemDataStore {
        [SerializeField]
        private string _title;
        [SerializeField]
        private BillingManager.PurchaseProducts _price;
        [SerializeField]
        private ItemStack _reward;
        
        public BillingManager.PurchaseProducts Price => _price;
        public ItemStack Reward => _reward;
        public string Title => _title;
    }
}