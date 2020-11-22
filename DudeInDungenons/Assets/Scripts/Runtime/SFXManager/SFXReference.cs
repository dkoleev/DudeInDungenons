using UnityEngine;

namespace Runtime.SFXManager {
    public class SFXReference : MonoBehaviour {
        [SerializeField]
        private SFX _sfx;

        public void PlaySFX() {
            _sfx.Play();
        }
    }
}