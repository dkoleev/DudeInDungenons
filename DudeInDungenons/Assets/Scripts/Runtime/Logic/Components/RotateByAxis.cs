using UnityEngine;

namespace Runtime.Logic.Components {
    public class RotateByAxis {
        private Transform _target;
        private float _rotateSpeed;
        public RotateByAxis(Transform target, float rotateSpeed) {
            _target = target;
            _rotateSpeed = rotateSpeed;
        }

        public void Rotate(Vector2 axis) {
            if (axis.magnitude < 0.001f) {
                return;
            }

            var move = new Vector3(axis.x, 0, axis.y);
            if (move.magnitude > 1f) {
                move.Normalize();
            }

            var forward = _target.forward;
            var angleCurrent = Mathf.Atan2( forward.x, forward.z) * Mathf.Rad2Deg;
            var targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            var deltaAngle = Mathf.DeltaAngle(angleCurrent, targetAngle);
            var targetLocalRot = Quaternion.Euler(0, deltaAngle, 0);
            var targetRotation = Quaternion.Slerp(Quaternion.identity, targetLocalRot, _rotateSpeed * Time.deltaTime);

            _target.rotation *= targetRotation;
        }
    }
}