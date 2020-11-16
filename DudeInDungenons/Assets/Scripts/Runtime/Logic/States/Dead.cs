using UnityEngine.AI;

namespace Runtime.Logic.States {
    public class Dead : BaseState {
        public Dead(NavMeshAgent agent) : base(agent) { }

        public override void Enter() {
            base.Enter();
            Agent.isStopped = true;
        }
    }
}