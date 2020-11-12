using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Ui.World;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class Player : Entity, ILocalPositionAdapter, IWeaponOwner, IDamagable {
        [SerializeField, Required, AssetsOnly] 
        [InlineEditor(InlineEditorModes.GUIOnly)]
        private PlayerData _data;
        [SerializeField, Required]
        private Transform _shootRaycastStartPoint;

        public Vector3 LocalPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public Transform RotateTransform => _rotateTransform;
        public Transform MainTransform => _mainTransform;

        private WorldBar _healthBar;
        private MoveByController _mover;
        private AttackComponent _attackComponent;
        private RotateByAxis _rotator;
        private FindTargetByDistance _findTargetByDistance;
        
        private Transform _rotateTransform;
        private Transform _mainTransform;
        private PlayerProgress _progress;
        private bool _initialized;

        protected override void Awake() {
            base.Awake();

            _progress = Progress.Player;
            
            _mainTransform = transform;
            _rotateTransform = _mainTransform.Find("Root");
            if (_progress.Health == 0) {
                _progress.Health = _data.MaxHealth;
            }
            
            _attackComponent = new AttackComponent("Pistol", this);
            AddComponent(_attackComponent);
            _mover = new MoveByController(this, _data.SpeedMove);
            AddComponent(_mover);
            _rotator = new RotateByAxis(_rotateTransform, _data.SpeedRotate);
            AddComponent(_rotator);
            _findTargetByDistance = new FindTargetByDistance(_mainTransform);
            AddComponent(_findTargetByDistance);
            
            _healthBar = GetComponentInChildren<WorldBar>();
            _healthBar.Initialize(_progress.Health, _data.MaxHealth);
        }

        protected override void Start() {
            base.Start();
            
            _initialized = true;
        }

        protected override void Update() {
            base.Update();
            
            if (!_initialized) {
                return;
            }

            _mover.Update();
            if (_mover.IsMoving) {
                _findTargetByDistance.SetTarget(null);
                _rotator.Rotate(_mover.MoveAxis);
            } else {
                _findTargetByDistance.Update();
                if (_findTargetByDistance.CurrentTarget != null) {
                    _attackComponent?.Update(_findTargetByDistance.CurrentTarget);
                }
            }
            
            _mainTransform.localRotation = Quaternion.Euler(new Vector3(0, _mainTransform.localRotation.y, 0));
        }
        
        public void TakeDamage(int damage) {
            _progress.Health -= damage;
            if (_progress.Health <= 0) {
                _progress.Health = 0;
                Dead();
            }
            
            _healthBar.SetProgress(_progress.Health);
        }

        private void Dead() {
            Debug.Log("Player dead");
        }
    }
}