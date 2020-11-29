using System.Collections.Generic;
using Runtime.Data;
using Runtime.Data.Settings;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.UI.Base;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Runtime.UI.MainMenu.Equipment {
    public class PetsShop : UiBase {
        [SerializeField, AssetsOnly, Required]
        private PetShopItem _scrollItemPrefab;
        [SerializeField, Required]
        private GridLayoutGroup _grid;
        [SerializeField, Required]
        private UiButton _buyButton;
        [SerializeField, Required]
        private Button _selectButton;
        [SerializeField, Required]
        private Button _backButton;
        
        public Relay OnBackClick = new Relay();
        
        private List<PetShopItem> _scrollItems = new List<PetShopItem>();
        private PetShopItem _selectedItem;
        private PetsSettingsData _petsSettings;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            _petsSettings = GameController.SettingsReference.Pets;

            FillScroll();
            UpdateView();
            LoadCurrentPet();
            
            _backButton.onClick.AddListener(CloseWindow);
            _buyButton.Button.onClick.AddListener(BuyPet);
            _selectButton.onClick.AddListener(SelectPet);
        }

        public override void SetActive(bool isActive) {
            base.SetActive(isActive);

            if (isActive) {
                LoadCurrentPet();
            }
        }

        private void LoadCurrentPet() {
            var progress = GameController.Progress.Player;
            if (!string.IsNullOrEmpty(progress.CurrentPet)) {
               // EventBus<OnCurrentPetChangedInShop>.Raise(new OnCurrentPetChangedInShop(progress.CurrentPet));
            }
            
            EventBus<OnCurrentPetChangedInShop>.Raise(new OnCurrentPetChangedInShop(_selectedItem.Data.Asset));
        }

        private void FillScroll() {
            foreach (var petData in _petsSettings.Pets) {
                var scrollItem = Instantiate(_scrollItemPrefab, Vector3.zero, Quaternion.identity, _grid.transform);
                scrollItem.transform.localPosition = Vector3.zero;

                var selected = GameController.Progress.Player.CurrentPet;
                if (petData.Asset.AssetGUID == selected) {
                    _selectedItem = scrollItem;
                    scrollItem.SetContent(petData, true);
                } else {
                    scrollItem.SetContent(petData, false);
                }

                _scrollItems.Add(scrollItem);
            }

            foreach (var petShopItem in _scrollItems) {
                petShopItem.OnSelected.AddListener(OnItemSelected);
            }
        }

        private void UpdateView() {
            var selectedId = _selectedItem.Data.Asset.AssetGUID;
            var currentPetIsBought =
                GameController.Progress.Player.UnlockedPets.Contains(selectedId);
            var petIsCurrentSelected = selectedId == GameController.Progress.Player.CurrentPet;
            
            _selectButton.gameObject.SetActive(currentPetIsBought && !petIsCurrentSelected);
            _buyButton.gameObject.SetActive(!currentPetIsBought);
            _buyButton.SetIcon(_selectedItem.Data.Price.Item.Icon);
            _buyButton.SetText(_selectedItem.Data.Price.Amount.ToString());
        }

        private void BuyPet() {
            GameController.Progress.Player.UnlockedPets.Add(_selectedItem.Data.Asset.AssetGUID);
            GameController.Progress.Player.CurrentPet = _selectedItem.Data.Asset.AssetGUID;
            
            UpdateView();
        }

        private void SelectPet() {
            GameController.Progress.Player.CurrentPet = _selectedItem.Data.Asset.AssetGUID;
            
            UpdateView();
        }

        private void OnItemSelected(PetShopItem item) {
            _selectedItem = item;

            foreach (var shopItem in _scrollItems) {
                shopItem.SetSelected(shopItem == item);
            }

            UpdateView();
        }

        private void CloseWindow() {
            OnBackClick.Dispatch();
        }
    }
}