using Runtime.Game.Entities;
using Runtime.Logic;
using Runtime.Static;
using UnityEngine;

namespace Runtime.Game {
    public class Portal : Entity {
        private ParticleSystem _effect;
        private Stage _stage;
        private BoxCollider _collider;

        protected override void Start() {
            base.Start();
            
            _effect = GetComponentInChildren<ParticleSystem>();
            _collider = GetComponent<BoxCollider>();
            Disable();
        }

        public void SetContent(Stage stage) {
            _stage = stage;
        }

        public void Activate() {
            _effect.gameObject.SetActive(true);
            _collider.enabled = true;
            _effect.Play(true);
        }

        private void Disable() {
            _effect.gameObject.SetActive(false);
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag(EntityTag.Player.ToString())) {
                _stage.LoadNextLevel();
            }
        }
    }
}
