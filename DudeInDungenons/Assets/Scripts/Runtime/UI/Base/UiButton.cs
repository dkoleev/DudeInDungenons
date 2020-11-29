using Runtime.Utilities;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Base {
    [RequireComponent (typeof (Button))]
    public class UiButton : MonoBehaviour {
        [SerializeField]
        private bool _withIcon;
        [SerializeField]
        [ShowIf("_withIcon")]
        private Image _icon;
        [SerializeField]
        private bool _withText;
        [SerializeField]
        [ShowIf("_withText")]
        private TextMeshProUGUI _text;
        [SerializeField, Required]
        private Image _background;
        [SerializeField, Required, AssetsOnly]
        private Sprite _enabledBackground;
        [SerializeField, Required, AssetsOnly]
        private Sprite _disableBackground;

        public Button Button { get; private set; }

        protected virtual void Awake() {
            Button = GetComponent<Button>();
            SetEnabled(true);
        }

        public void SetEnabled(bool isEnabled) {
            Button.interactable = isEnabled;
            _background.sprite = isEnabled ? _enabledBackground : _disableBackground;
        }

        public void SetIcon(Texture2D icon) {
            _icon.sprite = LoadHelper.CreateSprite(icon);
        }
        
        public void SetIcon(Sprite icon) {
            _icon.sprite = icon;
        }

        public void SetText(string text) {
            _text.text = text;
        }
    }
}
