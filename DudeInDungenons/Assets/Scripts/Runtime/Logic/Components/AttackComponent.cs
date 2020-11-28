using Runtime.Logic.WeaponSystem;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Logic.Components {
    public class AttackComponent : IComponent {
        public Relay OnShoot = new Relay();
        public bool Initialized { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        
        private float _currentShootDelay = 0;
        private IWeaponOwner _owner;
        private readonly float _rotationSpeed;
        private bool _isRotating;

        public AttackComponent() {
            
        }

        public AttackComponent(ResourceId weaponId, IWeaponOwner owner, float rotationSpeed = 600) {
            _owner = owner;
            _rotationSpeed = rotationSpeed;
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

            /*var canAttack = Vector3.Distance(target.MainTransform.position, _owner.RotateTransform.position) <= CurrentWeapon.Range;
            if (!canAttack) {
                return;
            }*/

            RotateToTarget(target);

            _currentShootDelay -= Time.deltaTime;
            if (_currentShootDelay <= 0 && !_isRotating) {
                _currentShootDelay = CurrentWeapon.ShootDelay;
                Shoot(target);
            }
        }

        private void RotateToTarget(IDamagable target) {
            var targetDirection = (target.MainTransform.position - _owner.RotateTransform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);

            var rotation = _owner.RotateTransform.rotation;
            var angle = Quaternion.Angle(rotation, targetRotation);
            _isRotating = angle > 10f;
            
            rotation = Quaternion.RotateTowards(rotation, targetRotation , Time.deltaTime * _rotationSpeed);
            _owner.RotateTransform.rotation = rotation;
        }

        private void Shoot(IDamagable target) {
            CurrentWeapon.Shoot(target);
            OnShoot.Dispatch();
        }
        
        private void CreateWeapon(ResourceId weaponId) {
            var weaponPlacer = _owner.MainTransform.GetComponentInChildren<WeaponPlacer>();
            Addressables.InstantiateAsync(weaponId.ToString(), weaponPlacer.transform).Completed += OnLoad;
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