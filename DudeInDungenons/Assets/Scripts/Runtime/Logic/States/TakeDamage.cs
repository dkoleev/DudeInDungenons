using UnityEngine.AI;

namespace Runtime.Logic.States {
    public class TakeDamage : BaseState {
        public TakeDamage(NavMeshAgent agent) : base(agent) {
            
        }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}