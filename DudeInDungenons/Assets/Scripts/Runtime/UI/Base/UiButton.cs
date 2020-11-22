using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Base {
    [RequireComponent (typeof (Button))]
    public class UiButton : MonoBehaviour {
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
    }
}
