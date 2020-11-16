using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Logic.WeaponSystem {
    public class HitScan : Weapon {
        [SerializeField]
        private Transform _shootEffectTransform;
        [SerializeField, AssetsOnly]
        private GameObject _shootEffect;
        
        public override void Shoot(IDamagable target) {
            target.TakeDamage(Damage);
            PlayShootEffect();
        }

        private void PlayShootEffect() {
            if (_shootEffectTransform is null || _shootEffect is null) {
                return;
            }

            var go = Instantiate(_shootEffect, _shootEffectTransform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            var effects = go.GetComponentsInChildren<ParticleSystem>();
            foreach (var effect in effects) {
                effect.Play();
            }
            
            Destroy(go, 2.0f);
        }
    }
}
