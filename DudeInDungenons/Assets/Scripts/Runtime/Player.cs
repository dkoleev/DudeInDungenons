using System;
using System.Collections.Generic;
using Avocado.Framework.Patterns.StateMachine;
using Runtime.Data;
using Runtime.Data.Items;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Logic.States.Player;
using Runtime.Static;
using Runtime.Ui.World;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class Player : Entity, ILocalPositionAdapter, IWeaponOwner, IDamagable,
        IEventReceiver<OnEnemyDead> {
        
        public enum PlayerState {
            Idle,
            Run,
            Attack
        }
        
        [SerializeField, Required, AssetsOnly, InlineEditor] 
        private PlayerData _data;
        [SerializeField]
        public EntityTag _attackTarget;
        [SerializeField, Required]
        private Transform _shootRaycastStartPoint;
        
        public Relay<IState, IState> OnStateChanged = new Relay<IState, IState>();

        public Vector3 LocalPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public Transform RotateTransform => _rotateTransform;
        public Transform MainTransform => _mainTransform;
        public bool IsMoving => _mover.IsMoving;

        private WorldBar _healthBar;
        private MoveByController _mover;
        private AttackComponent _attackComponent;
        private RotateByAxis _rotator;
        private FindTargetByDistance _findTargetByDistance;
        private PlayerVisual _visual;
        
        private Transform _rotateTransform;
        private Transform _mainTransform;
        private PlayerProgress PlayerProgress => Progress.Player;
        private int _health;

        private StateMachine _stateMachine;
        private IState _attackState;
        
        private bool _initialized;

        protected override void Awake() {
            base.Awake();
            EventBus.Register(this);

            _health = _data.MaxHealth;
            _mainTransform = transform;
            _rotateTransform = _mainTransform.Find("Root");
            
            _visual = new PlayerVisual(this);
            _attackComponent = new AttackComponent("Pistol", this);
            AddComponent(_attackComponent);
            _mover = new MoveByController(this, _data.SpeedMove);
            AddComponent(_mover);
            _rotator = new RotateByAxis(_rotateTransform, _data.SpeedRotate);
            AddComponent(_rotator);
            _findTargetByDistance = new FindTargetByDistance(_mainTransform, _attackTarget);
            AddComponent(_findTargetByDistance);
            
            _healthBar = GetComponentInChildren<WorldBar>();
            _healthBar.Initialize(_health, _data.MaxHealth);

            _attackComponent.OnShoot.AddListener(OnShoot);
        }
        
        protected override void Start() {
            base.Start();
            
            InitializeFsm();
            _visual.Initialize();
            
            _initialized = true;
        }

        private void InitializeFsm() {
            _stateMachine = new StateMachine();
            var idleState = new PlayerIdleState();
            var moveState = new PlayerMoveState(_rotator, _mover, _findTargetByDistance);
            _attackState = new PlayerAttackState(_attackComponent, _findTargetByDistance);
            
            _stateMachine.SetState(idleState);
            
            To(_attackState, () => !_mover.IsMoving && _findTargetByDistance.CurrentTargetIsAvailable());
            To(moveState, () => _mover.IsMoving);
            
            void To(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

            _stateMachine.OnStateChanged += (prevState, newState) => {
                OnStateChanged.Dispatch(prevState, newState);
            };
        }

        protected override void Update() {
            base.Update();
            
            if (!_initialized) {
                return;
            }

            _mover.Update();
            _findTargetByDistance.Update();
            _stateMachine.Tick();
            _visual.Update();
            
            _mainTransform.localRotation = Quaternion.Euler(new Vector3(0, _mainTransform.localRotation.y, 0));
        }
        
        private void OnShoot() {
            _stateMachine.SetState(_attackState);
            _visual.UpdateVisualByState(null, _attackState);
        }
        
        public void TakeDamage(int damage) {
            _health -= damage;
            if (_health <= 0) {
                _health = 0;
                Dead();
            }
            
            _healthBar.SetProgress(_health);
        }

        private void Dead() {
            Debug.Log("Player dead");
        }

        public void OnEvent(OnEnemyDead e) {
            AddToInventory(e.Enemy.Data.Drop);
        }

        private void AddToInventory(List<ItemStack> drop) {
            var inventory = PlayerProgress.Inventory;
            foreach (var itemStack in drop) {
                var id = itemStack.Item.Id;
                if (inventory.ContainsKey(id)) {
                    inventory[id] += itemStack.Amount;
                } else {
                    inventory.Add(id, itemStack.Amount);
                }
            }
        }

        private void OnDestroy() {
            _attackComponent.OnShoot.RemoveListener(OnShoot);
        }
    }
}