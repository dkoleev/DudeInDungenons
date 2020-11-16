using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiIdle : AiBase {
        public AiIdle(NavMeshAgent agent) : base(agent) {
        }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}