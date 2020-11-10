using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Runtime.Logic.WeaponSystem {
    public class Pistol : Weapon {
        public override int Damage => 5;
        public override float ShootDelay => 1;

        public override void Shoot() {
            RaycastHit hit;
            if (Physics.Raycast(Owner.RaycastStartPoint.position, Owner.RaycastStartPoint.forward, out hit, 100)) {
                var targets = hit.transform.GetComponentsInParent<MonoBehaviour>();
                foreach (var target in targets) {
                    if (target is IDamagable damagable) {
                        damagable.TakeDamage(Damage);
                        break;
                    }
                }
            }
            DrawRay();
        }

        [Conditional("DEBUG")]
        private void DrawRay() {
            Debug.DrawRay(Owner.RaycastStartPoint.position, Owner.RaycastStartPoint.forward, Color.green, 1.0f);
        }
    }
}