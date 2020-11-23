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

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
        }
    }
}