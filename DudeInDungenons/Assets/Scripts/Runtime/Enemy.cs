using Runtime.Data;
using Runtime.Logic;
using Runtime.Ui.World;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : MonoBehWrapper, IDamagable {
        [SerializeField] private EnemyData _data;
        
        private WorldBar _healthBar;
        private int _currentHealth;
        private NavMeshAgent _agent;
        private RandomMove _mover;

        protected override void Initialize() {
            base.Initialize();
            
            _currentHealth = _data.MaxHealth;
            _agent = GetComponent<NavMeshAgent>();
            _healthBar = GetComponentInChildren<WorldBar>();
            
            _mover = new RandomMove(_agent);
            _mover.Move();
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
        }

        private void Death() {
            Destroy(gameObject);
        }
    }
}