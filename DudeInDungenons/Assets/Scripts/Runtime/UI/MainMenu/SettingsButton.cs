using Runtime.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu {
    public class SettingsButton : UiBase {
        [SerializeField]
        private Button _openButton;
        [SerializeField]
        private UiBase _settingsWindow;

        private void Awake() {
            _openButton.onClick.AddListener(ShowSettingWindow);
        }

        private void ShowSettingWindow() {
            _settingsWindow.Show();
        }
    }
}