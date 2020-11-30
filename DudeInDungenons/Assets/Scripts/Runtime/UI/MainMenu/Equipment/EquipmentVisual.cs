using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.Static;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.UI.MainMenu.Equipment {
    public class EquipmentVisual : MonoBehaviour, IEventReceiver<OnCurrentPetChangedInShop> {
        [SerializeField, Required]
        private Transform _petParent;
        
        private Pet _currentPet;
        private AssetReference _currentAsset;
        private GameController _gameController;

        private bool _isLoading;

        private void Awake() {
            EventBus.Register(this);
            
            _gameController = GameObject.FindWithTag(EntityTag.GameController.ToString()).GetComponent<GameController>();
            Initialize();
        }

        private void Initialize() {
            var asset = GetPetAssetById(_gameController.Progress.Player.CurrentPet);
            if (!(asset is null)) {
                LoadPet(asset);
            }
        }

        private AssetReference GetPetAssetById(string id) {
            foreach (var petData in _gameController.SettingsReference.Pets.Pets) {
                if (id == petData.Asset.AssetGUID) {
                    return petData.Asset;
                }
            }

            return null;
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
                _currentPet.transform.parent = _petParent;
                _currentPet.transform.localPosition = Vector3.zero;
                _currentPet.transform.localRotation = Quaternion.Euler(Vector3.zero);

                _isLoading = false;
            };
        }

        public void OnEvent(OnCurrentPetChangedInShop e) {
            var currentPetState = _gameController.Progress.Player.CurrentPet;
            if (string.IsNullOrEmpty(currentPetState) || currentPetState != e.PetAsset.AssetGUID) {
                return;
            }

            if (_currentPet == null || _currentPet.Asset.AssetGUID != e.PetAsset.AssetGUID) {
                LoadPet(e.PetAsset);
            }
        }
        
        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}