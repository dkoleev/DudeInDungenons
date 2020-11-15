using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Sigtrap.Relays;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : Entity, IDamagable, IWeaponOwner, ITarget{
        public enum EnemyState {
            Idle,
            Run,
            Attack,
            TakeDamage,
            Dead
        }
        
        [SerializeField, Required, AssetsOnly]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        private EnemyData _data;
        [SerializeField, Required] 
        private Transform _shootRaycastStartPoint;
        
        public Relay<EnemyState> OnStateChanged = new Relay<EnemyState>();
        
        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public bool IsReachable => CurrentState != EnemyState.Dead;
        public Transform Transform => transform;
        public Transform RotateTransform => transform;
        public Transform MainTransform => transform;

        public NavMeshAgent NavMeshAgent => _agent;
        public int CurrentHealth => _currentHealth;
        public EnemyData Data => _data;
        
        public Relay<float> OnHealthChanged = new Relay<float>();
        public Relay OnDead = new Relay();
        private EnemyAi AI;
        
        private int _currentHealth;
        private NavMeshAgent _agent;
        private RandomMove _mover;
        private EnemyVisual _visual;
        private Player _player;
        private AttackComponent _attackComponent;
        private bool _isAttack;
        private float _currentTakeDamageDelay;
        public EnemyState CurrentState { get; private set; }

        private bool _initialized;

        protected override void Awake() {
            base.Awake();
            
            _currentHealth = _data.MaxHealth;
            _agent = GetComponent<NavMeshAgent>();
            _visual = new EnemyVisual(this);

            _attackComponent = new AttackComponent(_data.Weapon.name, this);
            AddComponent(_attackComponent);
            AI = new EnemyAi(this);
            AddComponent(AI);
        }

        protected override void Start() {
            base.Start();

            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            AI.SetTarget(_player);
            _visual.Initialize();

            _initialized = true;
        }
        
        protected override void Update() {
            base.Update();
            
            if (!_initialized || CurrentState == EnemyState.Dead) {
                return;
            }

            var takeDamage = _currentTakeDamageDelay > 0;
            if (takeDamage) {
                _currentTakeDamageDelay -= Time.deltaTime;
            }
            
            AI.Update(takeDamage);
            UpdateAttack();
            UpdateState();
        }

        private void UpdateAttack() {
            if (AI.IsAttack) {
                if (!_isAttack) {
                    _attackComponent?.Reset();
                }

                _attackComponent?.Update(_player);
            }

            _isAttack = AI.IsAttack;
        }
        
        public void TakeDamage(int damage) {
            if (CurrentState == EnemyState.Dead) {
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

        private void UpdateState() {
            if (_currentTakeDamageDelay > 0) {
                ChangeCurrentState(EnemyState.TakeDamage, true);
                return;
            }

            if (AI.IsAttack) {
                ChangeCurrentState(EnemyState.Attack, true);
            } else {
                ChangeCurrentState(EnemyState.Run);
            }
        }

        private void ChangeCurrentState(EnemyState state, bool sendSameState = false) {
            if (CurrentState != state || sendSameState) {
                CurrentState = state;
                OnStateChanged.Dispatch(CurrentState);
            }
        }

        private void Death() {
            ChangeCurrentState(EnemyState.Dead);

            EventBus<OnEnemyDead>.Raise(new OnEnemyDead(this));
            OnDead.Dispatch();
        }
    }
}