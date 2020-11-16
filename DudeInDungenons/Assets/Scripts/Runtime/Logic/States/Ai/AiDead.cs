using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiDead : AiBase {
        public AiDead(NavMeshAgent agent) : base(agent) { }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}