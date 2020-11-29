﻿using Runtime.Utilities;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class InventoryItem : MonoBehaviour {
        [SerializeField, Required]
        private Image _icon;
        [SerializeField, Required]
        private TextMeshProUGUI _amount;

        public void Initialize(Texture2D icon, string amount) {
            _icon.sprite = LoadHelper.CreateSprite(icon);
            _amount.text = amount;
        }
    }
}