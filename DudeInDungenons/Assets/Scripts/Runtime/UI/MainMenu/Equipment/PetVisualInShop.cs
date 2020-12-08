using Runtime.Game.Entities.Pet;
using Runtime.Utilities;
using Runtime.Visual;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.UI.MainMenu.Equipment {
    public class PetVisualInShop : MonoBehaviour {
        [SerializeField, Required]
        private Transform _petTransformParent;

        private PetsShop _shop;

        private Pet _currentPet;
        private AssetReference _currentAsset;

        private bool _isLoading;

        private void Start() {
            _shop = FindObjectOfType<PetsShop>();
            _shop.OnItemSelected.AddListener(SkinSelected);
        }

        private void SkinSelected(ItemsShopItem skinItem) {
            if (_currentPet == null || _currentPet.Data.Asset.AssetGUID != skinItem.Data.Id) {
                LoadPet(skinItem.Data.Asset);
            }
        }

        private void LoadPet(AssetReference asset) {
            if (_isLoading) {
                return;
            }

            _currentAsset?.ReleaseInstance(_currentPet.gameObject);

            _currentAsset = asset;

            _isLoading = true;
            
            _currentAsset.InstantiateAsync().Completed += handle => {
                _currentPet = handle.Result.GetComponent<Pet>();
                _currentPet.transform.parent = _petTransformParent;
                _currentPet.transform.localPosition = Vector3.zero;
                _currentPet.transform.localRotation = Quaternion.Euler(Vector3.zero);

                _isLoading = false;
            };
        }
    }
}