using UnityEngine;

namespace Avocado.Logic {
    public class MoveByAxis {
        public void Move(Vector2 axis, float speed, Transform transform) {
            if (axis.magnitude > 0) {
                transform.position += new Vector3(
                    axis.x * Time.deltaTime * speed,
                    0,
                    axis.y * Time.deltaTime * speed);
            }
        }
    }
}