using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.MainMenu {
    public class InventoryItem : MonoBehaviour {
        [SerializeField, Required]
        private Image _icon;
        [SerializeField, Required]
        private TextMeshProUGUI _amount;

        public void Initialize(Texture2D icon, string amount) {
            _icon.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));
            _amount.text = amount;
        }
    }
}