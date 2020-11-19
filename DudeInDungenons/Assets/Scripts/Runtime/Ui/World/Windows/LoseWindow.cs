using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui;
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
            _continueAdsButton.onClick.AddListener(ContinueLevel);
            _continueResButton.onClick.AddListener(ContinueLevel);
        }

        private void LeaveLevel() {
            EventBus.Raise(new OnExitLevelClick());
        }

        private void ContinueLevel() {
            EventBus.Raise(new OnContinueLevelClick());
            Hide();
        }
    }
}