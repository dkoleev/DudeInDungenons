using Runtime.Logic;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui;
using Runtime.Logic.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World.Windows {
    public class LoseWindow : WindowBase {
        [Title("Buttons")]
        [SerializeField, Required]
        private Button _closeButton;
        [SerializeField, Required]
        private Button _continueAdsButton;
        [SerializeField, Required]
        private Button _continueResButton;

        private void Awake() {
            _closeButton.onClick.AddListener(LeaveLevel);
            _continueAdsButton.onClick.AddListener(ContinueByADS);
            _continueResButton.onClick.AddListener(ContinueByRes);
        }

        private void LeaveLevel() {
            EventBus<OnExitLevelClick>.Raise(new OnExitLevelClick());
        }

        private void ContinueByRes() {
            var res = GameController.Inventory.SpendResource(ResourceId.Gem, 12);
            if (res == Inventory.InventoryOperationResult.NoEnoughResource) {
                //TODO: show shop to by resources
            }

            EventBus<OnContinueLevelClick>.Raise(new OnContinueLevelClick());
            Hide();
        }

        private void ContinueByADS() {
            EventBus<OnContinueLevelClick>.Raise(new OnContinueLevelClick());
            Hide();
        }
    }
}