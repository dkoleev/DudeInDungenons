using Runtime.Static;
using UnityEngine;

namespace Runtime {
    public class Portal : Entity {
        private ParticleSystem _effect;
        private Level _level;

        protected override void Start() {
            base.Start();
            
            _effect = GetComponentInChildren<ParticleSystem>();
            Disable();
        }

        public void SetContent(Level level) {
            _level = level;
        }

        public void Activate() {
            _effect.gameObject.SetActive(true);
            _effect.Play(true);
        }

        private void Disable() {
            _effect.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag(EntityTag.Player.ToString())) {
                _level.LoadNextLevel();
            }
        }
    }
}
