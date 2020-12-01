using System.Collections.Generic;
using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.UI.Base;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class ItemsShop : UiBase {
        [SerializeField]
        protected ItemsReference.ItemType _type;
        [SerializeField, AssetsOnly, Required]
        private ItemsShopItem _scrollItemPrefab;
        [SerializeField, Required]
        private GridLayoutGroup _grid;
        [SerializeField, Required]
        private UiButton _buyButton;
        [SerializeField, Required]
        private Button _selectButton;
        [SerializeField, Required]
        private Button _backButton;
        
        public Relay<ItemsShopItem> OnItemSelected = new Relay<ItemsShopItem>();
        public Relay OnBackClick = new Relay();
        public Relay<string> OnNeedResources = new Relay<string>();
        
        protected List<ItemsShopItem> _scrollItems = new List<ItemsShopItem>();
        protected ItemsShopItem _selectedItem;
        private List<ItemAction> _items = new List<ItemAction>();

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            var items = itemsReference.GetItems(_type);
            foreach (var item in items) {
                _items.Add(item as ItemAction);
            }

            FillScroll();
            UpdateView();
            LoadCurrentItem();
            
            _backButton.onClick.AddListener(CloseWindow);
            _buyButton.Button.onClick.AddListener(BuyItem);
            _selectButton.onClick.AddListener(SelectItem);
        }

        public override void SetActive(bool isActive) {
            base.SetActive(isActive);

            if (isActive) {
                LoadCurrentItem();
                UpdateView();
                ItemSelected(_selectedItem);
            }
        }

        protected virtual void LoadCurrentItem() {
            
        }

        private void FillScroll() {
            var selected = GetCurrentItem();
            
            foreach (var item in _items) {
                var scrollItem = Instantiate(_scrollItemPrefab, Vector3.zero, Quaternion.identity, _grid.transform);
                scrollItem.transform.localPosition = Vector3.zero;

                if (!string.IsNullOrEmpty(selected) && item.Id == selected) {
                    _selectedItem = scrollItem;
                    scrollItem.SetContent(_type, item, true);
                } else {
                    scrollItem.SetContent(_type, item, false);
                }
                
                scrollItem.SetCurrent(selected);

                _scrollItems.Add(scrollItem);
            }
            
            if (string.IsNullOrEmpty(selected)) {
                _selectedItem = _scrollItems[0];
            }

            foreach (var shopItem in _scrollItems) {
                shopItem.OnSelected.AddListener(ItemSelected);
            }

            _selectedItem.SelectItem();
        }

        private void UpdateView() {
            var selectedId = _selectedItem.Data.Id;
            var currentItemIsBought = GetUnlockedItems().Contains(selectedId);
            var currentItem = GetCurrentItem();
            var itemIsCurrentSelected = selectedId == currentItem;
            
            _scrollItems.ForEach(item => item.SetCurrent(currentItem));
            
            _selectButton.gameObject.SetActive(currentItemIsBought && !itemIsCurrentSelected);
            _buyButton.gameObject.SetActive(!currentItemIsBought);

            var price = _selectedItem.Data.Action.Price[0];
            _buyButton.SetIcon(price.Item.Icon);
            _buyButton.SetText(price.Amount.ToString());
        }

        protected virtual string GetCurrentItem() {
            return string.Empty;
        }
        
        protected virtual void SetCurrentItem(string id) {
            
        }

        protected virtual HashSet<string> GetUnlockedItems() {
            return new HashSet<string>();
        }

        private void BuyItem() {
            if (_selectedItem.Data.Action.Pay(GameController.Inventory)) {
                UnlockItem();
            } else {
                OnNeedResources.Dispatch(_selectedItem.Data.Action.Price[0].Item.Id);
            }

            UpdateView();
        }

        protected virtual void UnlockItem() {
            
        }

        private void SelectItem() {
            SetCurrentItem(_selectedItem.Data.Id);
            _selectedItem.SelectItem();
            EventBus<OnCurrentItemChangedInShop>.Raise(new OnCurrentItemChangedInShop(_type, _selectedItem.Data));
            CloseWindow();
        }

        private void ItemSelected(ItemsShopItem item) {
            _selectedItem = item;

            foreach (var shopItem in _scrollItems) {
                shopItem.SetSelected(shopItem == item);
            }

            UpdateView();
            
            OnItemSelected.Dispatch(_selectedItem);
        }

        private void CloseWindow() {
            OnBackClick.Dispatch();
        }
    }
}