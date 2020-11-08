using Runtime.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime {
    public class Enemy : MonoBehaviour {
        private NavMeshAgent _agent;
        private RandomMove _mover;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _mover = new RandomMove(_agent);
            _mover.Move();
        }

        private void Update() {
            if (_mover.IsTargetReached()) {
                _mover.Move();
            }
        }
    }
}