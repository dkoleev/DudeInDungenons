using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class MoveByController :
        IEventReceiver<OnMovePlayer>, 
        IEventReceiver<OnMovePerformed>,
        IEventReceiver<OnMoveCancelled> {
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
        }

        public void Update() {
            Move();
        }
        
        private void Move() {
            if (_moveAxis.magnitude > 0) {
                _target.LocalPosition += new Vector3(
                    _moveAxis.x * Time.deltaTime * _speed,
                    0,
                    _moveAxis.y * Time.deltaTime * _speed);
            }
        }
        
        public void OnEvent(OnMovePerformed e) {
            _moveAxis = e.Axis;
        }

        public void OnEvent(OnMoveCancelled e) {
            _moveAxis = Vector2.zero;
        }
        
        public void OnEvent(OnMovePlayer e) {
            _isMoving = e.IsMove;
        }
    }
}