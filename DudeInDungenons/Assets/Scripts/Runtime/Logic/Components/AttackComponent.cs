using System;
using Runtime.Logic.WeaponSystem;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class AttackComponent : IComponent {
        public Action OnShoot;
        private Weapon _currentWeapon;
        private float _currentShootDelay = 0;

        
        public AttackComponent(Weapon weapon) {
            _currentWeapon = weapon;
            Reset();
        }

        public void Reset() {
            _currentShootDelay = _currentWeapon.ShootDelay;
        }

        public void Initialize() {
            
        }

        public void Update() {
            _currentShootDelay -= Time.deltaTime;
            if (_currentShootDelay <= 0) {
                Reset();
                Shoot();
            }
        }

        private void Shoot() {
            _currentWeapon.Shoot();
            OnShoot?.Invoke();
        }
    }
}
