using System.Linq;
using Runtime.Logic;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {
    public class ResourcesPanel : UiBase, 
        IEventReceiver<OnAddResourceToInventory>,
        IEventReceiver<OnSpendResources> {
        [SerializeField, Required]
        private TextMeshProUGUI _gemAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _goldAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _energyAmount;
        [SerializeField, Required]
        private TextMeshProUGUI _levelAmount;
        [SerializeField, Required]
        private Slider _expProgress;
        
        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
            EventBus.Register(this);
            
            UpdateView();
        }

        private void UpdateView() {
            _gemAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Gem).ToString();
            _goldAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Gold).ToString();
            _energyAmount.text = GameController.Inventory.GetResourceAmount(ResourceId.Energy).ToString();
            UpdateExp();
        }

        private void UpdateExp() {
            var level = GameController.Inventory.GetResourceAmount(ResourceId.Level);
            var curExp = GameController.Inventory.GetResourceAmount(ResourceId.Exp);
            var levelingModel = GameController.SettingsReference.LevelUp.LevelByExp;

            var needExp = 0;
            needExp = level >= levelingModel.Last().Key ? curExp : levelingModel[level + 1];
            
            _levelAmount.text = (level + 1).ToString();
            _expProgress.value = Mathf.Clamp01(curExp / (float) needExp);
        }

        public void OnEvent(OnAddResourceToInventory e) {
            UpdateView();
        }
        
        public void OnEvent(OnSpendResources e) {
            UpdateView();
        }

        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}