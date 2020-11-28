using UnityEngine;

namespace Runtime.Ui.World {
    public class AlignByCamera : MonoBehaviour {
        private Camera _mainCamera;

        void Start() {
            _mainCamera = Camera.main;
        }

        void Update() {
            AlignCamera();
        }
        
        private void AlignCamera() {
            var camXform = _mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}