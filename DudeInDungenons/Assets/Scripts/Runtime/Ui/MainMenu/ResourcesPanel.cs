using Runtime.Logic;
using Runtime.Logic.GameProgress;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.Ui.MainMenu {
    public class ResourcesPanel : UiBase {
        [SerializeField, Required]
        private TextMeshProUGUI _gemAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _goldAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _energyAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _expAmount;
        
        public override void Initialize(GameProgress progress, ItemsReference itemsReference) {
            base.Initialize(progress, itemsReference);

            _gemAmount.text = Progress.Player.GetResourceAmount(ResourceId.Gem).ToString();
            _goldAmount.text = Progress.Player.GetResourceAmount(ResourceId.Gold).ToString();
            _energyAmount.text = Progress.Player.GetResourceAmount(ResourceId.Energy).ToString();
            _expAmount.text = Progress.Player.GetResourceAmount(ResourceId.Exp).ToString();
        }
    }
}