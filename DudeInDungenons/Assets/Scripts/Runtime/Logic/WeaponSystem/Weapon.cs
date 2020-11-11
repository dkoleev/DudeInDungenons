using Runtime.Data;
using UnityEngine;

namespace Runtime.Logic.WeaponSystem {
    public abstract class Weapon : MonoBehaviour {
        [SerializeField] protected WeaponData Data;
        
        public float ShootDelay => Data.ShootDelay;
        public float Range => Data.Range;

        protected int Damage => Data.Damage;
        protected IWeaponOwner Owner;
        
        public virtual void Initialize(IWeaponOwner owner) {
            Owner = owner;
        }

        public virtual void Shoot(IDamagable target) {
            target.TakeDamage(Damage);
        }
        public abstract void Shoot();
    }
}