using Avocado.Framework.Patterns.StateMachine;
using UnityEngine.AI;

namespace Runtime.Logic.States {
    public class BaseState : IState {
        protected NavMeshAgent Agent;

        protected BaseState(NavMeshAgent agent) {
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
