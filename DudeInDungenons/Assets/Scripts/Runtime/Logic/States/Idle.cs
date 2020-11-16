using UnityEngine.AI;

namespace Runtime.Logic.States {
    public class Idle : BaseState {
        public Idle(NavMeshAgent agent) : base(agent) {
        }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}