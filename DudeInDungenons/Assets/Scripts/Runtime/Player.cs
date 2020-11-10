using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.WeaponSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime {
    public class Player : MonoBehaviour, ILocalPositionAdapter, IWeaponOwner {
        [SerializeField] private PlayerData _data;
        [SerializeField] private Transform _shootRaycastStartPoint;
        
        public Vector3 LocalPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Transform RaycastStartPoint => _shootRaycastStartPoint;

        private Enemy _currentTarget;
        private MoveByController _mover;
        private AttackComponent _attackComponent;
        private Transform _rotateTransform;
        private bool _initialized;
    
        private void Awake() {
            _mover = new MoveByController(this, _data.SpeedMove);
            _rotateTransform = transform.Find("Root");
            CreateWeapon();
        }

        private void Update() {
            if (!_initialized) {
                return;
            }

            _mover.Update();
            if (_mover.IsMoving) {
                _currentTarget = null;
                Rotate(_rotateTransform, 5);
            } else {
                _attackComponent.Update();
                if (_currentTarget == null) {
                    var enemies = FindObjectsOfType<Enemy>();
                    foreach (var enemy in enemies) {
                        if (_currentTarget == null) {
                            _currentTarget = enemy;
                        } else if(enemy != _currentTarget &&
                            Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, _currentTarget.transform.position)) {
                            _currentTarget = enemy;
                        }
                    }
                }

                if (_currentTarget != null) {
                    _rotateTransform.LookAt(_currentTarget.transform);
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
                _attackComponent = new AttackComponent(weapon);
                _attackComponent.OnShoot += () => {
                };

                _initialized = true;
            }
        }

        private void Rotate(Transform target, float speed) {
            if (_mover.MoveAxis.magnitude < 0.001f) {
                return;
            }

            var move = new Vector3(_mover.MoveAxis.x, 0, _mover.MoveAxis.y);
            if (move.magnitude > 1f) {
                move.Normalize();
            }

            var forward = target.forward;
            var angleCurrent = Mathf.Atan2( forward.x, forward.z) * Mathf.Rad2Deg;
            var targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            var deltaAngle = Mathf.DeltaAngle(angleCurrent, targetAngle);
            var targetLocalRot = Quaternion.Euler(0, deltaAngle, 0);
            var targetRotation = Quaternion.Slerp(Quaternion.identity, targetLocalRot, speed * Time.deltaTime);

            target.rotation *= targetRotation;
        }
    }
}