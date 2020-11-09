using Runtime.Input;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.WeaponSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime {
    public class Player : MonoBehaviour, ILocalPositionAdapter, IWeaponOwner {
        [SerializeField] private float _speedMove;
        [SerializeField] private Transform _shootRaycastStartPoint;
        
        public Vector3 LocalPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Transform RaycastStartPoint => _shootRaycastStartPoint;

        private Vector2 _moveAxis;
        private MoveByAxis _mover;
        private AttackComponent _attackComponent;
        private Transform _rotateTransform;
        private bool _initialized;
    
        private void Awake() {
            _mover = new MoveByAxis();
            _moveAxis = Vector2.zero;
            _rotateTransform = transform.Find("Root");
            CreateWeapon();
        
            InputManager.Instance.MainControl.Player.Move.performed += MoveOnPerformed;
            InputManager.Instance.MainControl.Player.Move.canceled += MoveOnCanceled; 
            InputManager.Instance.MainControl.Player.Enable();
        }

        private void Update() {
            if (!_initialized) {
                return;
            }

            _mover.Move(_moveAxis, _speedMove, this);
            if (_mover.IsMoving) {
                Rotate(_rotateTransform, 5);
            } else {
                _attackComponent.Update();
                var enemy = Object.FindObjectOfType<Enemy>();
                if (enemy != null) {
                    _rotateTransform.LookAt(enemy.transform);
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

        private void Attack() {
            
        }

        private void MoveOnCanceled(InputAction.CallbackContext obj) {
            _moveAxis = Vector2.zero;
        }

        private void MoveOnPerformed(InputAction.CallbackContext context) {
            _moveAxis = context.ReadValue<Vector2>();
        }
        
        private void Rotate(Transform target, float speed) {
            if (_moveAxis.magnitude < 0.001f) {
                return;
            }

            var move = new Vector3(_moveAxis.x, 0, _moveAxis.y);
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