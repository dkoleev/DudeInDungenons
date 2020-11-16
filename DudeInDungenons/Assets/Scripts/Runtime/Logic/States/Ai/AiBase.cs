using Avocado.Framework.Patterns.StateMachine;
using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiBase : IState {
        protected NavMeshAgent Agent;

        protected AiBase(NavMeshAgent agent) {
            Agent = agent;
        }

        public virtual void Tick() {
        }

        public virtual void Enter() {
        }

        public virtual void Exit() {
        }
    }
}
