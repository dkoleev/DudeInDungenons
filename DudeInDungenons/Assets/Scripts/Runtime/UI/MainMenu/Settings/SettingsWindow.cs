using Runtime.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Settings {
    public class SettingsWindow : UiWindow {
        [SerializeField]
        private Button _selectLanguageButton;
        [SerializeField]
        private LanguagesWindow _languagesWindow;

        protected override void Awake() {
            base.Awake();
            Hide();
            _selectLanguageButton.onClick.AddListener(OpenLanguageWindow);
        }

        private void OpenLanguageWindow() {
            _languagesWindow.Show();
        }
    }
}