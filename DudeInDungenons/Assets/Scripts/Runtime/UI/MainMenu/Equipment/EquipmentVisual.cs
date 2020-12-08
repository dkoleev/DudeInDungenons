using Runtime.Data.Items;
using Runtime.Game.Entities.Pet;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.Static;
using Runtime.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.UI.MainMenu.Equipment {
    public class EquipmentVisual : MonoBehaviour, IEventReceiver<OnCurrentItemChangedInShop> {
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
                if (id == petData.Id) {
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
            LoadHelper.InstantiateAsset<Pet>(_currentAsset, pet => {
                _currentPet = pet;
                _isLoading = false;
            }, _petParent);
        }
        
        public void OnEvent(OnCurrentItemChangedInShop e) {
            switch (e.ItemType) {
                case ItemsReference.ItemType.Pets:
                    ChangePet(e.Data);
                    break;
            }
        }

        private void ChangePet(Item item) {
            var currentPetState = _gameController.Progress.Player.CurrentPet;
            if (string.IsNullOrEmpty(currentPetState) || currentPetState != item.Id) {
                return;
            }

            if (_currentPet == null || _currentPet.Data.Asset.AssetGUID != item.Id) {
                LoadPet(item.Asset);
            }
        }

        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}