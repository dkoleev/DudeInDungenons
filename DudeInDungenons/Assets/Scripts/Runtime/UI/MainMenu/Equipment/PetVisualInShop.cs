using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.UI.MainMenu.Equipment {
    public class PetVisualInShop : MonoBehaviour, IEventReceiver<OnCurrentPetChangedInShop> {
        [SerializeField, Required]
        private Transform _petTransformParent;

        private Pet _currentPet;
        private AssetReference _currentAsset;

        private bool _isLoading;

        private void Awake() {
            EventBus.Register(this);
        }

        public void OnEvent(OnCurrentPetChangedInShop e) {
            if (_currentPet == null || _currentPet.Asset.AssetGUID != e.PetAsset.AssetGUID) {
                LoadPet(e.PetAsset);
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

        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}
