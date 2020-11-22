using Runtime.Logic;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.UI.MainMenu {
    public class ResourcesPanel : UiBase {
        [SerializeField, Required]
        private TextMeshProUGUI _gemAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _goldAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _energyAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _expAmount;
        
        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            _gemAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Gem).ToString();
            _goldAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Gold).ToString();
            _energyAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Energy).ToString();
            _expAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Exp).ToString();
        }
    }
}