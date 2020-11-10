using Runtime.Input;
using Runtime.Ui;
using UnityEngine;

namespace Runtime {
    public class GameRunner : MonoBehaviour {
        private UiManager _uiManager;
        private InputManager _inputManager;
        
        private void Awake() {
            _inputManager = new InputManager();
        }
    }
}