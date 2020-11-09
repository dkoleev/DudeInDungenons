using UnityEngine;

namespace Runtime.Logic.WeaponSystem {
    public abstract class Weapon : MonoBehaviour {
        protected IWeaponOwner Owner;
        
        public virtual void Initialize(IWeaponOwner owner) {
            Owner = owner;
        }
        
        public abstract int Damage { get; }
        public abstract float ShootDelay { get; }
        public abstract void Shoot();
    }
}