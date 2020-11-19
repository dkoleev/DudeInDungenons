using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World.Windows {
    public class PauseWindow : WindowBase {
        [SerializeField, Required]
        private Button _continueButton;
        [SerializeField, Required]
        private Button _giveUpButton;

        private void Awake() {
            _continueButton.onClick.AddListener(Hide);
            _giveUpButton.onClick.AddListener(ExitLevel);
        }

        private void ExitLevel() {
            Time.timeScale = 1.0f;
            EventBus.Raise(new OnExitLevelClick());
        }
    }
}