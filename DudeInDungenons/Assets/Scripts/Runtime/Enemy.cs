using System;
using System.Collections.Generic;
using System.Linq;
using Avocado.Framework.Patterns.StateMachine;
using Avocado.UnityToolbox.Timer;
using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.States.Ai;
using Runtime.Visual;
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
        [SerializeField, Required] 
        private Transform _deadEffectsParent;
        [SerializeField, Required] 
        private Transform _afterDeadEffectsParent;
        [SerializeField, AssetsOnly]
        private Effect _deadEffect;
        [SerializeField, AssetsOnly]
        private Effect _dissappearEffect;
        
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
        private bool _isDead;
        private bool _takeDamage;
        private float _currentTakeDamageDelay;
        private Transform _root;
        private Soul _soul;
        private Collider[] _colliders;
        
        private StateMachine _stateMachine;
        private IState _attackState;
        private TimeManager _timeManager;

        private bool _initialized;

        protected override void Awake() {
            base.Awake();

            _root = Transform.Find("Root") ?? Transform;
            _currentHealth = _data.MaxHealth;
            _agent = GetComponent<NavMeshAgent>();
            _colliders = GetComponentsInChildren<Collider>();
            _soul = GetComponentInChildren<Soul>();
            _agent.speed = Data.SpeedMove;
            _agent.angularSpeed = Data.SpeedRotate;
            
            _timeManager = new TimeManager();
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
                _visual.UpdateVisualByState(null, _attackState);
            });

            _initialized = true;
        }

        private void InitializeFsm() {
            _stateMachine = new StateMachine();

            var idleState = new AiIdle(_agent);
            var moveState = new AiMove(_agent);
            var deadState = new AiDead(_agent);
            var getDamageState = new AiTakeDamage(_agent);
            _attackState = new AiAttack(_agent, _attackComponent, _player);
            
            _agent.destination = _player.LocalPosition;

            _stateMachine.SetState(idleState);
            
            To(moveState, CanMove);
            To(deadState, () => _isDead);
            To(_attackState, CanAttack);
            To(getDamageState, () => !_isDead && _takeDamage);
            To(idleState, ()=> !CanMove() && !CanAttack() && !GetDamage() && !_isDead);
            
            void To(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            
            _stateMachine.OnStateChanged += (prevState, newState) => {
                OnStateChanged.Dispatch(prevState, newState);
            }; 
        }

        private bool GetDamage() {
            return !_isDead && _takeDamage;
        }

        private bool CanMove() {
            return !TargetReached() && !_takeDamage && !_isDead;
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

        private void Death() {
            _isDead = true;
            DisableCollider();
            
            EventBus<OnEnemyDead>.Raise(new OnEnemyDead(this));
            OnDead.Dispatch();
            
            PlayDeadEffect();
        }

        private void DisableCollider() {
            foreach (var col in _colliders) {
                col.enabled = false;
            }
        }

        private void PlayDeadEffect() {
            if (_deadEffect is null) {
                return;
            }

            var effect = Instantiate(_deadEffect, _deadEffectsParent);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localRotation = Quaternion.identity;
            Destroy(effect, 1.5f);
            _timeManager.Call(1.95f, () => {
                Instantiate(_dissappearEffect, _afterDeadEffectsParent.position, Quaternion.identity);
                _soul.Activate();
            });
            
            Destroy(gameObject, 2.0f);
        }

        private void OnDestroy() {
            _timeManager.Dispose();
        }
    }
}