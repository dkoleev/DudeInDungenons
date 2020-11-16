using UnityEngine;

namespace Runtime.Utilities {
    public class DelayDestroy : MonoBehaviour {
        [SerializeField]
        private float _delay;
        private void Start() {
            Destroy(gameObject, _delay);
        }
    }
}