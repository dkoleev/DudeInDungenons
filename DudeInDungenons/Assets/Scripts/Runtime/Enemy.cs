using System;
using Avocado.Framework.Patterns.StateMachine;
using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.States;
using Runtime.Logic.States.Ai;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : Entity, IDamagable, IWeaponOwner, ITarget {
        [SerializeField, Required, AssetsOnly, InlineEditor]
        private EnemyData _data;
        [SerializeField, Required] 
        private Transform _shootRaycastStartPoint;
        
        public Relay<IState, IState> OnStateChanged = new Relay<IState, IState>();
        public Relay<float> OnHealthChanged = new Relay<float>();
        public Relay OnDead = new Relay();
        
        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public bool IsReachable => !_isDead;
        public Transform Transform => transform;
        public Transform RotateTransform => transform;
        public Transform MainTransform => transform;
        public NavMeshAgent NavMeshAgent => _agent;
        public int CurrentHealth => _currentHealth;
        public EnemyData Data => _data;
        
        private int _currentHealth;
        private NavMeshAgent _agent;
        private RandomMove _mover;
        private EnemyVisual _visual;
        private Player _player;
        private AttackComponent _attackComponent;
        private bool _isAttack;
        private bool _isDead;
        private bool _takeDamage;
        private float _currentTakeDamageDelay;
        
        private StateMachine _stateMachine;

        private bool _initialized;

        protected override void Awake() {
            base.Awake();
            
            _currentHealth = _data.MaxHealth;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = Data.SpeedMove;
            _agent.angularSpeed = Data.SpeedRotate;
            
            _visual = new EnemyVisual(this);

            _attackComponent = new AttackComponent(_data.Weapon.name, this);
            AddComponent(_attackComponent);
        }

        protected override void Start() {
            base.Start();

            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            _visual.Initialize();
            InitializeFsm();

            _attackComponent.OnShoot.AddListener(() => {
                
            });

            _initialized = true;
        }

        private void InitializeFsm() {
            _stateMachine = new StateMachine();

            var idleState = new AiIdle(_agent);
            var moveState = new AiMove(_agent);
            var deadState = new AiDead(_agent);
            var getDamageState = new AiTakeDamage(_agent);
            var attackState = new AiAttack(_agent);
            
            _agent.destination = _player.LocalPosition;

            _stateMachine.SetState(idleState);
            
            Func<bool> CanMove() => () => !TargetReached() && !_takeDamage && !_isDead;
            
            To(moveState, CanMove());
            To(deadState, () => _isDead);
            To(attackState, CanAttack);
            To(getDamageState, () => !_isDead && _takeDamage);
            //At(_moveState, _attackState, () => _isAttack);
            
            void To(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            
            _stateMachine.OnStateChanged += (prevState, newState) => {
                OnStateChanged.Dispatch(prevState, newState);
            }; 
        }

        private bool CanAttack() {
            return TargetReached() && !_takeDamage && !_isDead;
        }

        protected override void Update() {
            base.Update();
            
            if (!_initialized) {
                return;
            }
            
            _stateMachine.Tick();
            
            if (_isDead) {
                return;
            }

            _takeDamage = _currentTakeDamageDelay > 0;
            if (_takeDamage) {
                _currentTakeDamageDelay -= Time.deltaTime;
            }
            
            _agent.destination = _player.LocalPosition;
            UpdateAttack();
        }

        private void UpdateAttack() {
            if (CanAttack()) {
                if (!_isAttack) {
                    _attackComponent?.Reset();
                }

                _attackComponent?.Update(_player);
            }

            _isAttack = CanAttack();
        }
        
        public void TakeDamage(int damage) {
            if (_isDead) {
                return;
            }

            _currentTakeDamageDelay = _data.TakeDamageDelay;

            _currentHealth -= damage;
            if (_currentHealth <= 0) {
                _currentHealth = 0;
                Death();
            }
            
            OnHealthChanged.Dispatch(_currentHealth);
        }

        private void Death() {
            _isDead = true;

            EventBus<OnEnemyDead>.Raise(new OnEnemyDead(this));
            OnDead.Dispatch();
        }
        
        private bool TargetReached() {
            if (!_agent.pathPending) {
                if (_agent.remainingDistance <= _agent.stoppingDistance) {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude <= 0f) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}