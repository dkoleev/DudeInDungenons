using Runtime.Data.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "Skin", menuName = "Data/Player skin")]
    public class PlayerSkinData : ScriptableObject {
        [SerializeField]
        private AssetReference _asset;
        [SerializeField]
        private ItemStack _price;
        [SerializeField]
        private Texture2D _icon;

        public string Id => _asset.AssetGUID;
        public AssetReference Asset => _asset;
        public ItemStack Price => _price;
        public Texture2D Icon => _icon;
    }
}
