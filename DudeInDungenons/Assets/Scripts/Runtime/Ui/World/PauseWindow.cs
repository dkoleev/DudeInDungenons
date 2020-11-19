using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class PauseWindow : MonoBehaviour {
        [SerializeField, Required]
        private Button _continueButton;
        [SerializeField, Required]
        private Button _giveUpButton;

        private void Awake() {
            _continueButton.onClick.AddListener(ResumeGame);
            _giveUpButton.onClick.AddListener(ExitLevel);
        }

        private void ResumeGame() {
            Time.timeScale = 1.0f;
            Hide();
        }

        private void ExitLevel() {
            Time.timeScale = 1.0f;
            EventBus.Raise(new OnExitLevelClick());
        }

        public void Show() {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        private void Hide() {
            gameObject.SetActive(false);
        }
    }
}