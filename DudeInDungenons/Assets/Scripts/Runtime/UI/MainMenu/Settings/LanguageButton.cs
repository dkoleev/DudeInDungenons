using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenu.Settings {
    public class LanguageButton : MonoBehaviour {
        [SerializeField]
        private SystemLanguage _language;

        [SerializeField]
        private Button _selectButton;

        private void Awake() {
            _selectButton.onClick.AddListener(() => {
                if (LocalizationManager.HasLanguage(_language.ToString())) {
                    LocalizationManager.CurrentLanguage = _language.ToString();
                }
            });
        }
    }
}