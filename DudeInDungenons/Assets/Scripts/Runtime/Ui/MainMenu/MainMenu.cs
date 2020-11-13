using Runtime.Logic.GameProgress;
using UnityEngine;

namespace Runtime.Ui.MainMenu {
    public class MainMenu : UiBase {
        [SerializeField]
        private Inventory _inventory;

        public override void Initialize(GameProgress progress, ItemsReference itemsReference) {
            base.Initialize(progress, itemsReference);
            
            _inventory.Initialize(Progress, ItemsReference);
        }
    }
}