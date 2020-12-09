using Avocado.Framework.Patterns.StateMachine;

namespace Runtime.Game.Entities.AI {
    public class AIBase {
        protected StateMachine StateMachine;

        public AIBase() {
            StateMachine = new StateMachine();
        }
    }
}
