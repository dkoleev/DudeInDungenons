using Avocado.UnityToolbox.Optimization.Pool;
using Avocado.UnityToolbox.Timer;
using Runtime.Visual;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Logic.WeaponSystem {
    public class HitScan : Weapon {
        [SerializeField]
        private Transform _shootEffectTransform;
        [SerializeField, AssetsOnly]
        private Effect _shootEffect;
        [SerializeField]
        private int _shootEffectPoolStartSize = 5;

        private Pool<Effect> _shootEffectPool;
        private TimeManager _timeManager;

        protected override void Start() {
            base.Start();
            
            _timeManager = new TimeManager();
            _shootEffectPool = new Pool<Effect>(_shootEffect, _shootEffectPoolStartSize, _shootEffectTransform);
        }

        public override void Shoot(IDamagable target) {
            target.TakeDamage(Damage);
            PlayShootEffect();
        }

        private void PlayShootEffect() {
            if (_shootEffectTransform is null || _shootEffect is null) {
                return;
            }

            var go = _shootEffectPool.Get();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            var effects = go.GetComponentsInChildren<ParticleSystem>();
            foreach (var effect in effects) {
                effect.Play();
            }

            _timeManager.Call(1.0f, () => _shootEffectPool.Release(go));
        }
    }
}
