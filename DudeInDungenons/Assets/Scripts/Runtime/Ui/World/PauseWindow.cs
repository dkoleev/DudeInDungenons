using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class PauseWindow : MonoBehaviour {
        [SerializeField, Required]
        private Button _resumeButton;

        private void Awake() {
            _resumeButton.onClick.AddListener(ResumeGame);
        }

        private void ResumeGame() {
            Time.timeScale = 1.0f;
            Hide();
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