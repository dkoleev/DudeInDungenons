using System.Linq;
using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Logic.GameProgress.Progress.Items;
using Runtime.UI.Base;
using Runtime.Utilities;
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
        private TextMeshProUGUI _energyTimeRestore;

        [SerializeField, Required]
        private TextMeshProUGUI _levelAmount;

        [SerializeField, Required]
        private Slider _expProgress;

        private ItemTimeProgress _energyProgress;
        private ItemRestoreByTime _energyData;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            EventBus.Register(this);

            _energyData = GameController.ItemReference.EnergyData;
            _energyProgress = GameController.Inventory.GetItem(_energyData.Id) as ItemTimeProgress;

            UpdateView();

            Initialized = true;
        }

        protected override void Update() {
            if (!Initialized) {
                return;
            }

            if (_energyProgress.Amount >= _energyData.MaxAmount) {
                _energyTimeRestore.enabled = false;
            } else {
                _energyTimeRestore.enabled = true;
                _energyTimeRestore.text = TimeUtils.GetTimeCaption(_energyProgress.Timer.Remaining,
                    TimeUtils.TimeCaptionDetails.MMSS);
            }
        }

        private void UpdateView() {
            _gemAmount.text = GameController.Inventory.GetResourceAmount(ItemsReference.GemData.Id).ToString();
            _goldAmount.text = GameController.Inventory.GetResourceAmount(ItemsReference.GoldData.Id).ToString();
            _energyAmount.text = GameController.Inventory.GetResourceAmount(ItemsReference.EnergyData.Id) + "/" +
                                 _energyData.MaxAmount;
            UpdateExp();
        }

        private void UpdateExp() {
            var level = GameController.Inventory.GetResourceAmount(ItemsReference.LevelData.Id);
            var curExp = GameController.Inventory.GetResourceAmount(ItemsReference.ExpData.Id);
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