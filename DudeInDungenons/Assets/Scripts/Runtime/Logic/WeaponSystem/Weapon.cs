using Runtime.Data;
using UnityEngine;

namespace Runtime.Logic.WeaponSystem {
    public abstract class Weapon : MonoBehaviour {
        [SerializeField] protected WeaponData Data;
        
        public float ShootDelay => Data.ShootDelay;

        protected int Damage => Data.Damage;
        protected IWeaponOwner Owner;
        
        public virtual void Initialize(IWeaponOwner owner) {
            Owner = owner;
        }

        public abstract void Shoot();
    }
}