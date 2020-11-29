using System.Collections.Generic;
using Runtime.Data;
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
        [SerializeField]
        private List<PetData> _pets;
        [SerializeField, AssetsOnly, Required]
        private PetShopItem _scrollItemPrefab;
        [SerializeField, Required]
        private GridLayoutGroup _grid;
        [SerializeField, Required]
        private Button _backButton;
        
        private List<PetShopItem> _scrollItems = new List<PetShopItem>();
        
        public Relay OnBackClick = new Relay();

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            LoadCurrentPet();
            FillScroll();
            
            _backButton.onClick.AddListener(CloseWindow);
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
            
            EventBus<OnCurrentPetChangedInShop>.Raise(new OnCurrentPetChangedInShop(_pets[0].Asset));
        }

        private void FillScroll() {
            var selected = true;
            foreach (var petData in _pets) {
                var scrollItem = Instantiate(_scrollItemPrefab, Vector3.zero, Quaternion.identity, _grid.transform);
                scrollItem.transform.localPosition = Vector3.zero;
                scrollItem.SetContent(petData, selected);
                selected = false;
                
                _scrollItems.Add(scrollItem);
            }

            foreach (var petShopItem in _scrollItems) {
                petShopItem.OnSelected.AddListener(OnItemSelected);
            }
        }

        private void OnItemSelected(PetShopItem item) {
            foreach (var shopItem in _scrollItems) {
                shopItem.SetSelected(shopItem == item);
            }
        }

        private void CloseWindow() {
            OnBackClick.Dispatch();
        }
    }
}