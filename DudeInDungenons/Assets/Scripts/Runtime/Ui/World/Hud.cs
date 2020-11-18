using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class Hud : MonoBehaviour {
        [SerializeField, Required]
        private Button _pauseButton;

        [SerializeField, Required]
        private PauseWindow _pauseWindow;
        
        private OnScreenStickCustom _mover;
        private void Awake() {
            _mover = GetComponentInChildren<OnScreenStickCustom>();
            
            _pauseButton.onClick.AddListener(ShowPauseMenu);
        }

        private void ShowPauseMenu() {
            _pauseWindow.Show();
        }
    }
}