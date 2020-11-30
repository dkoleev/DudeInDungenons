using Runtime.Data.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "Pet", menuName = "Data/Pet")]
    public class PetData : ScriptableObject {
        [SerializeField]
        private AssetReference _asset;
        [SerializeField]
        private float _speedMove;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private ItemStack _price;
        [SerializeField]
        private Texture2D _icon;

        public string Id => _asset.AssetGUID;
        public Texture2D Icon => _icon;
        public float SpeedMove => _speedMove;
        public float Damage => _damage;
        public ItemStack Price => _price;
        public AssetReference Asset => _asset;
    }
}