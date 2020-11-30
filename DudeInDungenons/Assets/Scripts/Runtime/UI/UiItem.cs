using System.Collections.Generic;
using Runtime.Logic;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI {
    public class UiItem : UiBase {
        [SerializeField, Required]
        private Image _icon;
        [SerializeField, Required]
        private Image _frame;
        [SerializeField, Required]
        private Image _highlight;
        [SerializeField, Required]
        private GameObject _bonus;
        [SerializeField, Required]
        private TextMeshProUGUI _amount;

        public bool IsActive { get; private set; }

        public void SetContent(KeyValuePair<string, int> dropItem) {
            var itemData = ItemsReference.GetItemById(dropItem.Key);
            
            _icon.sprite = Sprite.Create(itemData.Icon, new Rect(0, 0, itemData.Icon.width, itemData.Icon.height), Vector2.one / 2f);
            _amount.text = dropItem.Value.ToString();
        }

        public void SetActive(bool isActive) {
            IsActive = isActive;
            gameObject.SetActive(IsActive);
        }
    }
}