using Runtime.Data;
using Runtime.Logic;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : MonoBehWrapper, IDamagable {
        [SerializeField] private EnemyData _data;

        public int CurrentHealth => _currentHealth;
        public EnemyData Data => _data;
        
        public Relay<float> OnHealthChanged = new Relay<float>();
        public Relay OnDead = new Relay();
        
        private int _currentHealth;
        private NavMeshAgent _agent;
        private RandomMove _mover;
        private EnemyVisual _visual;

        protected override void Initialize() {
            base.Initialize();
            
            _currentHealth = _data.MaxHealth;
            _agent = GetComponent<NavMeshAgent>();
        
            _mover = new RandomMove(_agent);
            _mover.Move();
            _visual = new EnemyVisual(this);
        }
        
        private void Update() {
            if (_mover.IsTargetReached()) {
                _mover.Move();
            }
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