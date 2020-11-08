using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Avocado {
    public class Enemy : MonoBehaviour {
        private NavMeshAgent _agent;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            MoveToRandomPoint();
        }

        private void Update() {
            if (IsTargetReached()) {
                MoveToRandomPoint();
            }
        }

        private void MoveToRandomPoint() {
            var targetPoint = new Vector3(
                Random.Range(-10, 10),
                0,
                Random.Range(-10, 10));
            
            _agent.isStopped = false;
            _agent.destination = targetPoint;
        }

        private bool IsTargetReached() {
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