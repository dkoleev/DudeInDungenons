using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Base {
    public class UiButton : MonoBehaviour {
        [SerializeField, Required]
        private Image _background;
        [SerializeField, Required, AssetsOnly]
        private Sprite _enabledBackground;
        [SerializeField, Required, AssetsOnly]
        private Sprite _disableBackground;

        private Button _button;
        
        private void Awake() {
            _button = GetComponent<Button>();
            SetEnabled(true);
        }

        public void SetEnabled(bool isEnabled) {
            _button.interactable = isEnabled;
            _background.sprite = isEnabled ? _enabledBackground : _disableBackground;
        }
    }
}
