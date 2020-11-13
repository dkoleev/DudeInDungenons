using Runtime.Logic.GameProgress;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.MainMenu {
    public class MainMenu : UiBase {
        [SerializeField]
        private Inventory _inventory;
        [SerializeField]
        private Button _playButton;
        public Relay OnPlayClick = new Relay();

        public override void Initialize(GameProgress progress, ItemsReference itemsReference) {
            base.Initialize(progress, itemsReference);
            
            _inventory.Initialize(Progress, ItemsReference);
            _playButton.onClick.AddListener(() => {
                OnPlayClick.Dispatch();
            });
        }
    }
}