using Runtime.UI.Base;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Equipment {
    public class Pets : UiBase {
        [SerializeField, Required]
        private Button _backButton;
        
        public Relay OnBackClick = new Relay();

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
            _backButton.onClick.AddListener(CloseWindow);
        }

        private void CloseWindow() {
            OnBackClick.Dispatch();
        }
    }
}