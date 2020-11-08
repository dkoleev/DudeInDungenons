using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Logic {
    public class RandomMove {
        private NavMeshAgent _agent;

        public RandomMove(NavMeshAgent agent) {
            _agent = agent;
        }
        
        public void Move() {
            var targetPoint = new Vector3(
                Random.Range(-10, 10),
                0,
                Random.Range(-10, 10));
            
            _agent.isStopped = false;
            _agent.destination = targetPoint;
        }

        public bool IsTargetReached() {
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
