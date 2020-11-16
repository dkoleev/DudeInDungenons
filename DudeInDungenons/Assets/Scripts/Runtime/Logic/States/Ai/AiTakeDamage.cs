using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiTakeDamage : AiBase {
        public AiTakeDamage(NavMeshAgent agent) : base(agent) {
            
        }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}