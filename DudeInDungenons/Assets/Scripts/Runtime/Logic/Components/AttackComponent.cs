using Runtime.Logic.WeaponSystem;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Logic.Components {
    public class AttackComponent {
        public Relay OnShoot = new Relay();
        public bool Initialized { get; private set; }

        private Weapon _currentWeapon;
        private float _currentShootDelay = 0;
        private IWeaponOwner _owner;
        
        public AttackComponent(string weaponId, IWeaponOwner owner) {
            _owner = owner;
            CreateWeapon(weaponId);
        }

        public void Reset() {
            _currentShootDelay = 0;
        }

        public void Update(IDamagable target) {
            if (!Initialized) {
                return;
            }

            var canAttack = Vector3.Distance(target.MainTransform.position, _owner.RotateTransform.position) <= _currentWeapon.Range;
            if (!canAttack) {
                return;
            }

            _owner.RotateTransform.LookAt(target.MainTransform);

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
        
        private void CreateWeapon(string weaponId) {
            var weaponPlacer = _owner.MainTransform.GetComponentInChildren<WeaponPlacer>();
            Addressables.InstantiateAsync(weaponId, weaponPlacer.transform).Completed += OnLoad;
            void OnLoad(AsyncOperationHandle<GameObject> handle) {
                var go = handle.Result;
                var weapon = go.GetComponent<Weapon>();
                weapon.Initialize(_owner);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;

                _currentWeapon = weapon;
                _currentShootDelay = _currentWeapon.ShootDelay;

                Initialized = true;
            }
        }
    }
}