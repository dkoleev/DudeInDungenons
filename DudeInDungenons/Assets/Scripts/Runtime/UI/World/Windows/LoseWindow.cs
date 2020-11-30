using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui;
using Runtime.Logic.Inventory;
using Runtime.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World.Windows {
    public class LoseWindow : WindowBase {
        [Title("Buttons")]
        [SerializeField, Required]
        private Button _closeButton;
        [SerializeField, Required]
        private AdsButton _continueAdsButton;
        [SerializeField, Required]
        private Button _continueResButton;

        private void Awake() {
            _closeButton.onClick.AddListener(LeaveLevel);
            _continueResButton.onClick.AddListener(ContinueByRes);

            _continueAdsButton.OnAdsCompleted.AddListener(() => {
                EventBus<OnContinueLevelClick>.Raise(new OnContinueLevelClick());
                Hide();
            });
        }

        private void LeaveLevel() {
            Hide();
            EventBus<OnExitLevelClick>.Raise(new OnExitLevelClick());
        }

        private void ContinueByRes() {
            var res = GameController.Inventory.SpendResource(ItemsReference.GemData.Id, 12);
            if (res == Inventory.InventoryOperationResult.NoEnoughResource) {
                //TODO: show shop to by resources
                return;
            }
            
            EventBus<OnContinueLevelClick>.Raise(new OnContinueLevelClick());
            Hide();
        }
    }
}