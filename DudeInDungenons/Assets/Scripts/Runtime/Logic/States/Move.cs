using UnityEngine.AI;

namespace Runtime.Logic.States {
    public class Move : BaseState {
        public Move(NavMeshAgent agent) : base(agent) {
        }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = false;
        }
    }
}