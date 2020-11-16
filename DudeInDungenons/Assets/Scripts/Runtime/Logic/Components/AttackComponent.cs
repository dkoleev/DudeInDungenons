using Runtime.Logic.WeaponSystem;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Logic.Components {
    public class AttackComponent : IComponent {
        public bool CanAttack { get; private set; }
        public Relay OnShoot = new Relay();
        public bool Initialized { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        
        private float _currentShootDelay = 0;
        private IWeaponOwner _owner;
        private const float RotationSpeed = 600;
        
        public AttackComponent(string weaponId, IWeaponOwner owner) {
            _owner = owner;
            CreateWeapon(weaponId);
        }
        
        public void Initialize() {
            
        }

        public void Reset() {
            _currentShootDelay = 0;
        }

        public void Update(IDamagable target) {
            if (!Initialized || target == null) {
                return;
            }

            var canAttack = Vector3.Distance(target.MainTransform.position, _owner.RotateTransform.position) <= CurrentWeapon.Range;
            if (!canAttack) {
                return;
            }

            RotateToTarget(target);

            _currentShootDelay -= Time.deltaTime;
            if (_currentShootDelay <= 0) {
                _currentShootDelay = CurrentWeapon.ShootDelay;
                Shoot(target);
            }
        }

        private void RotateToTarget(IDamagable target) {
            if (Quaternion.Angle(_owner.RotateTransform.rotation, target.MainTransform.rotation) > 0.01f) {
                var targetDirection = (target.MainTransform.position - _owner.RotateTransform.position).normalized;
                var targetRotation = Quaternion.LookRotation(targetDirection);
                _owner.RotateTransform.rotation = Quaternion.RotateTowards(_owner.RotateTransform.rotation, targetRotation , Time.deltaTime * RotationSpeed);
            }
        }

        private void Shoot(IDamagable target) {
            CurrentWeapon.Shoot(target);
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

                CurrentWeapon = weapon;
                _currentShootDelay = CurrentWeapon.ShootDelay;

                Initialized = true;
            }
        }
    }
}