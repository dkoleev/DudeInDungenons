using System.Collections.Generic;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Runtime.Ui.World.Windows {
    public class RewardWindow : WindowBase {
        [SerializeField, Required, SceneObjectsOnly]
        private List<UiItem> _itemTemplates;
        [SerializeField, Required]
        private Button _collectButton;
        [SerializeField, Required]
        private AdsButton _collectAdsButton;

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            foreach (var uiItem in _itemTemplates) {
                uiItem.Initialize(GameController, ItemsReference);
            }

            _collectButton.onClick.AddListener(CollectReward);
            _collectAdsButton.OnAdsCompleted.AddListener(CollectRewardX2);
        }

        public override void Show() {
            base.Show();

            InitializeReward();
        }

        private void InitializeReward() {
            foreach (var uiItem in _itemTemplates) {
                uiItem.SetActive(false);
            }

            var i = 0;
            foreach (var drop in GameController.Player.Drop) {
                _itemTemplates[i].SetContent(drop);
                _itemTemplates[i].SetActive(true);
                i++;
            }
        }

        private void CollectReward() {
            GameController.CurrentLevel.MoveDropToInventory();
            Hide();
            EventBus<OnRewardCollected>.Raise(new OnRewardCollected());
        }
        
        private void CollectRewardX2() {
            GameController.CurrentLevel.MoveDropToInventoryX2();
            Hide();
            EventBus<OnRewardCollected>.Raise(new OnRewardCollected());
        }

        private void OnDestroy() {
            _collectButton.onClick.RemoveListener(CollectReward);
            _collectAdsButton.OnAdsCompleted.RemoveListener(CollectRewardX2);
        }
    }
}