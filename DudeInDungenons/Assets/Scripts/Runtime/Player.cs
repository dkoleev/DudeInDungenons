using System;
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

        private MoveByController _mover;
        private AttackComponent _attackComponent;
        private RotateByAxis _rotator;
        private LookAtTarget _lookAtTarget;
        
        private Transform _rotateTransform;
        private Transform _mainTransform;
        private bool _initialized;

        private void Awake() {
            _rotateTransform = transform.Find("Root");
        }

        private void Start() {
            _mainTransform = transform;
            _mover = new MoveByController(this, _data.SpeedMove);
            _rotator = new RotateByAxis(_rotateTransform, _data.SpeedRotate);
            _lookAtTarget = new LookAtTarget(_mainTransform);
            _lookAtTarget.Initialize();
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
                _attackComponent.Update();
                if (_lookAtTarget.CurrentTarget != null) {
                    _rotateTransform.LookAt(_lookAtTarget.CurrentTarget.transform);
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
                _attackComponent.Initialize();
                _attackComponent.OnShoot += () => {
                };

                _initialized = true;
            }
        }
    }
}