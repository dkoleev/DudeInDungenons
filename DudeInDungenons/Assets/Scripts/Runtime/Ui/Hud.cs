using UnityEngine;

namespace Runtime.Ui {
    public class Hud : MonoBehaviour {
        public bool StickIsPressed { get; private set; }
        
        private OnScreenStickCustom _mover;
        private void Awake() {
            _mover = GetComponentInChildren<OnScreenStickCustom>();
            _mover.OnPointerChangeState += isActive => {
                StickIsPressed = isActive;
            };
        }
    }
}