using Runtime.Logic.WeaponSystem;
using Sigtrap.Relays;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class AttackComponent {
        public Relay OnShoot = new Relay();
        
        private Weapon _currentWeapon;
        private float _currentShootDelay = 0;
        private IWeaponOwner _owner;
        
        public AttackComponent(Weapon weapon, IWeaponOwner owner) {
            _currentWeapon = weapon;
            _owner = owner;
            _currentShootDelay = _currentWeapon.ShootDelay;
        }

        public void Reset() {
            _currentShootDelay = 0;
        }

        public void Update(IDamagable target) {
            var canAttack = Vector3.Distance(target.Transform.position, _owner.RotateTransform.position) <= _currentWeapon.Range;
            if (!canAttack) {
                return;
            }

            _owner.RotateTransform.LookAt(target.Transform);

            _currentShootDelay -= Time.deltaTime;
            if (_currentShootDelay <= 0) {
                _currentShootDelay = _currentWeapon.ShootDelay;
                Shoot(target);
            }
        }

        private void Shoot(IDamagable target) {
            _currentWeapon.Shoot(target);
            OnShoot.Dispatch();
        }
    }
}