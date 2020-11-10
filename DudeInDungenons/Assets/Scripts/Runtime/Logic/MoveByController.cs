using Runtime.Input;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Logic {
    public class MoveByController : IEventReceiver<OnMovePlayer> {
        public bool IsMoving => _isMoving;
        public Vector2 MoveAxis => _moveAxis;
        
        private ILocalPositionAdapter _target;
        private Vector2 _moveAxis;
        private float _speed;
        private bool _isMoving;
        
        public MoveByController(ILocalPositionAdapter target, float speed) {
            _target = target;
            _speed = speed;
            _moveAxis = Vector2.zero;
            
            EventBus.Register(this);
            InputManager.Instance.MainControl.Player.Move.performed += MoveOnPerformed;
            InputManager.Instance.MainControl.Player.Move.canceled += MoveOnCanceled;
            InputManager.Instance.MainControl.Player.Move.Enable();
        }

        public void Update() {
            Move();
        }

        private void MoveOnCanceled(InputAction.CallbackContext obj) {
            _moveAxis = Vector2.zero;
        }

        private void MoveOnPerformed(InputAction.CallbackContext context) {
            _moveAxis = context.ReadValue<Vector2>();
        }
        
        private void Move() {
            if (_moveAxis.magnitude > 0) {
                _target.LocalPosition += new Vector3(
                    _moveAxis.x * Time.deltaTime * _speed,
                    0,
                    _moveAxis.y * Time.deltaTime * _speed);
            }
        }

        public void OnEvent(OnMovePlayer e) {
            Debug.Log("isMove " + e.IsMove);
            _isMoving = e.IsMove;
        }
    }
}