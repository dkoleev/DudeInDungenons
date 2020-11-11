using Runtime.Data;
using Runtime.Logic;
using Runtime.Logic.Components;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : MonoBehaviour, IDamagable, IWeaponOwner {
        [SerializeField] private EnemyData _data;
        [SerializeField] private Transform _shootRaycastStartPoint;
        
        public Transform RaycastStartPoint => _shootRaycastStartPoint;
        public Transform RotateTransform => transform;
        public Transform MainTransform => transform;

        public Transform Transform => transform;
        public NavMeshAgent NavMeshAgent => _agent;
        public int CurrentHealth => _currentHealth;
        public EnemyData Data => _data;
        
        public Relay<float> OnHealthChanged = new Relay<float>();
        public Relay OnDead = new Relay();
        
        private int _currentHealth;
        private NavMeshAgent _agent;
        private RandomMove _mover;
        private EnemyAi _ai;
        private EnemyVisual _visual;
        private Player _player;
        private AttackComponent _attackComponent;
        private bool _isAttack;

        private bool _initialized;

        private void Awake() {
            _currentHealth = _data.MaxHealth;
            _attackComponent = new AttackComponent("Bit", this);
            _agent = GetComponent<NavMeshAgent>();
            _visual = new EnemyVisual(this);
        }

        private void Start() {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            _ai = new EnemyAi(this, _player);

            _initialized = true;
        }
        
        private void Update() {
            if (!_initialized) {
                return;
            }
            
            _ai.Update();
            UpdateAttack();
        }

        private void UpdateAttack() {
            if (_ai.IsAttack) {
                if (!_isAttack) {
                    _attackComponent?.Reset();
                }

                _attackComponent?.Update(_player);
            }

            _isAttack = _ai.IsAttack;
        }
        
        public void TakeDamage(int damage) {
            _currentHealth -= damage;
            if (_currentHealth <= 0) {
                _currentHealth = 0;
                Death();
            }
            
            OnHealthChanged.Dispatch(_currentHealth);
        }

        private void Death() {
            OnDead.Dispatch();
            _visual.Dispose();
            
            Destroy(gameObject);
        }
    }
}