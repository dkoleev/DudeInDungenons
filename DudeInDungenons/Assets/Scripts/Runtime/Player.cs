using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.WeaponSystem;
using Runtime.Ui.World;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime {
    public class Player : MonoBehaviour, ILocalPositionAdapter, IWeaponOwner, IDamagable {
        [SerializeField] private PlayerData _data;
        [SerializeField] private Transform _shootRaycastStartPoint;
        
        public Vector3 LocalPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public Transform RotateTransform => _rotateTransform;
        public Transform Transform => _mainTransform;

        private WorldBar _healthBar;
        private MoveByController _mover;
        private AttackComponent _attackComponent;
        private RotateByAxis _rotator;
        private LookAtTarget _lookAtTarget;
        
        private Transform _rotateTransform;
        private Transform _mainTransform;
        private int _currentHealth;
        private bool _initialized;

        private void Awake() {
            _rotateTransform = transform.Find("Root");
        }

        private void Start() {
            _currentHealth = _data.MaxHealth;
            _mainTransform = transform;
            _mover = new MoveByController(this, _data.SpeedMove);
            _rotator = new RotateByAxis(_rotateTransform, _data.SpeedRotate);
            _lookAtTarget = new LookAtTarget(_mainTransform);
            _lookAtTarget.Initialize();
            _healthBar = GetComponentInChildren<WorldBar>();
            _healthBar.Initialize(_data.MaxHealth, _data.MaxHealth);
            
            CreateWeapon();
        }

        private void Update() {
            if (!_initialized) {
                return;
            }

            _mover.Update();
            if (_mover.IsMoving) {
                _lookAtTarget.SetTarget(null);
                _rotator.Rotate(_mover.MoveAxis);
            } else {
                _lookAtTarget.Update();
                if (_lookAtTarget.CurrentTarget != null) {
                    _attackComponent.Update(_lookAtTarget.CurrentTarget);
                }
            }
        }

        private void CreateWeapon() {
            var weaponPlacer = gameObject.GetComponentInChildren<WeaponPlacer>();
            Addressables.InstantiateAsync("Pistol", weaponPlacer.transform).Completed += OnLoad;
            void OnLoad(AsyncOperationHandle<GameObject> handle) {
                var go = handle.Result;
                var weapon = go.GetComponent<Weapon>();
                weapon.Initialize(this);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                _attackComponent = new AttackComponent(weapon, this);

                _initialized = true;
            }
        }
        
        public void TakeDamage(int damage) {
            _currentHealth -= damage;
            if (_currentHealth <= 0) {
                _currentHealth = 0;
                Dead();
            }
            
            _healthBar.SetProgress(_currentHealth);
        }

        private void Dead() {
            Debug.Log("Player dead");
        }
    }
}