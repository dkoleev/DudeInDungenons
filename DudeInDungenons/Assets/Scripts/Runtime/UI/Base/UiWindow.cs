using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Base {
    public class UiWindow : UiBase {
        [SerializeField]
        private Button[] _closeButtons;

        protected override void Awake() {
            base.Awake();

            for (int i = 0; i < _closeButtons.Length; i++) {
                _closeButtons[i].onClick.AddListener(Hide);

            }
        }
    }
}