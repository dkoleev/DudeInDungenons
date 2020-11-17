using Avocado.UnityToolbox.Optimization.Pool;
using UnityEngine;

namespace Runtime.Visual {
    public class Effect : MonoBehaviour, IPoolable {
        private ParticleSystem[] _effects;
        private void Awake() {
            _effects = gameObject.GetComponentsInChildren<ParticleSystem>();
        }

        public void Spawn() {
            gameObject.SetActive(true);
            foreach (var effect in _effects) {
                effect.Play();
            }
        }

        public void Release() {
            gameObject.SetActive(false);
            /*foreach (var effect in _effects) {
                effect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }*/
        }
    }
}