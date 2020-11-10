using UnityEngine;

namespace Runtime.Ui {
    public class Hud : MonoBehaviour {
        private OnScreenStickCustom _mover;
        private void Awake() {
            _mover = GetComponentInChildren<OnScreenStickCustom>();
        }
    }
}