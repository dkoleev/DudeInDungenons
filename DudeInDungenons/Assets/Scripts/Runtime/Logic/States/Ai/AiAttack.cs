using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiAttack : AiBase {
        public AiAttack(NavMeshAgent agent) : base(agent) { }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}