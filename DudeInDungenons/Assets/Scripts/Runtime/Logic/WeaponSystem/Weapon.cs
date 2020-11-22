using System;
using Runtime.Data;
using UnityEngine;

namespace Runtime.Logic.WeaponSystem {
    public abstract class Weapon : MonoBehaviour {
        [SerializeField] protected WeaponData Data;
        
        public ResourceId Id => Data.Id;
        public float ShootDelay => Data.ShootDelay;
        public float Range => Data.Range;
        public int AnimatorId => Data.AnimatorId;

        protected int Damage => Data.Damage;
        protected IWeaponOwner Owner;

        protected virtual void Start() {
            
        }

        public virtual void Initialize(IWeaponOwner owner) {
            Owner = owner;
        }

        public abstract void Shoot(IDamagable target);
    }
}