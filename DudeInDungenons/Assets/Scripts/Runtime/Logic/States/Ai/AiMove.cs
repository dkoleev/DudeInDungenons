using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiMove : AiBase {
        public AiMove(NavMeshAgent agent) : base(agent) {
        }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = false;
        }
    }
}