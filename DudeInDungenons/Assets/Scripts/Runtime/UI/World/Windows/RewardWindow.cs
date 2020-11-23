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

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
            _collectButton.onClick.AddListener(CollectReward);
        }

        private void CollectReward() {
            Hide();
            EventBus<OnRewardCollected>.Raise(new OnRewardCollected());
        }

        private void OnDestroy() {
            _collectButton.onClick.RemoveListener(CollectReward);
        }
    }
}