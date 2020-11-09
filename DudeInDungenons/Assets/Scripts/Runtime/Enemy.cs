using Runtime.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : MonoBehaviour, IDamagable {
        public int StartHeath = 10;
        public int CurrentHealth { get; set; }
        private NavMeshAgent _agent;
        private RandomMove _mover;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _mover = new RandomMove(_agent);
            _mover.Move();
            CurrentHealth = StartHeath;
        }

        private void Update() {
            if (_mover.IsTargetReached()) {
                _mover.Move();
            }
        }

        public void TakeDamage(int damage) {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0) {
                Death();
            }
        }

        private void Death() {
            CurrentHealth = 0;
            Destroy(gameObject);
        }
    }
}