using System.Collections.Generic;
using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;
using Runtime.Utilities;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class ItemsShopItem : MonoBehaviour {
        [SerializeField, Required]
        private Button _button;
        [SerializeField, Required]
        private Image _icon;
        [SerializeField, Required]
        private Image _currentFocus;
        [SerializeField, Required]
        private List<Image> _defaultBack;
        [SerializeField, Required]
        private List<Image> _selectedBack;

        public bool IsSelected { get; private set; }
        public ItemAction Data => _data;

        public Relay<ItemsShopItem> OnSelected = new Relay<ItemsShopItem>();

        private ItemAction _data;
        private ItemsReference.ItemType _type;

        private void Awake() {
            _button.onClick.AddListener(SelectItem);
        }

        public void SetContent(ItemsReference.ItemType type, ItemAction data, bool selected) {
            _data = data;
            _type = type;
            
            SetSelected(selected);
            
            _icon.sprite = LoadHelper.CreateSprite(_data.Icon);
        }

        public void SetCurrent(string currentItemId) {
            var isCurrent = !string.IsNullOrEmpty(currentItemId) && _data.Id == currentItemId;
            _currentFocus.enabled = isCurrent;
        }

        public void SetSelected(bool isSelected) {
            IsSelected = isSelected;
            
            _selectedBack.ForEach(image => image.enabled = IsSelected);
            _defaultBack.ForEach(image => image.enabled = !IsSelected);
        }
        
        public void SelectItem() {
            OnSelected.Dispatch(this);
        }
    }
}