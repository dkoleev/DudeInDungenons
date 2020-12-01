using Runtime.Visual;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.UI.MainMenu.Equipment {
    public class SkinsVisualInShop : MonoBehaviour {
        [SerializeField, Required]
        private PlayerOnIsland _player;

        private SkinsShop _skinsShop;

        private void Start() {
            _skinsShop = Object.FindObjectOfType<SkinsShop>();
            _skinsShop.OnItemSelected.AddListener(SkinSelected);
        }

        private void SkinSelected(ItemsShopItem skinItem) {
            _player.UpdateSkin(skinItem.Data);
        }

        private void OnDestroy() {
            _skinsShop.OnItemSelected.RemoveListener(SkinSelected);
        }
    }
}