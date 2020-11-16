using UnityEngine.AI;

namespace Runtime.Logic.States {
    public class Attack : BaseState {
        public Attack(NavMeshAgent agent) : base(agent) { }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}