using Avocado.Input;
using Avocado.Logic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Avocado {
    public class Player : MonoBehaviour {
        [SerializeField] private float _speedMove;
        private Vector2 _moveAxis;
        private MoveByAxis _mover;
    
        private void Awake() {
            _mover = new MoveByAxis();
            _moveAxis = Vector2.zero;
        
            InputManager.Instance.MainControl.Player.Move.performed += MoveOnPerformed;
            InputManager.Instance.MainControl.Player.Move.canceled += MoveOnCanceled; 
            InputManager.Instance.MainControl.Player.Enable();
        }

        private void Update() {
            _mover.Move(_moveAxis, _speedMove, transform);
        }

        private void MoveOnCanceled(InputAction.CallbackContext obj) {
            _moveAxis = Vector2.zero;
        }

        private void MoveOnPerformed(InputAction.CallbackContext context) {
            _moveAxis = context.ReadValue<Vector2>();
        }

    }
}