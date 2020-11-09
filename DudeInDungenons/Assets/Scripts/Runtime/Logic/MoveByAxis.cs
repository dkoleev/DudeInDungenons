using UnityEngine;

namespace Runtime.Logic {
    public class MoveByAxis {
        private Vector2 _moveAxis;
        public bool IsMoving => _moveAxis.x > 0 || _moveAxis.y > 0;

        public void Move(Vector2 axis, float speed, ILocalPositionAdapter localPosition) {
            _moveAxis = axis;
            if (_moveAxis.magnitude > 0) {
                localPosition.LocalPosition += new Vector3(
                    _moveAxis.x * Time.deltaTime * speed,
                    0,
                    _moveAxis.y * Time.deltaTime * speed);
            }
        }
    }
}