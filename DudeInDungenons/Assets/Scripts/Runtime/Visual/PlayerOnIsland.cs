using UnityEngine;

namespace Runtime.Visual {
    public class PlayerOnIsland : MonoBehaviour {
        private static readonly int Sitting = Animator.StringToHash("Sitting");

        private void Awake() {
            var animator = GetComponentInChildren<Animator>();
            animator.SetBool(Sitting, true);
        }
    }
}
