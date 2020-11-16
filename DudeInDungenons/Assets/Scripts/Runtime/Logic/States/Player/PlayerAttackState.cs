using Avocado.Framework.Patterns.StateMachine;
using Runtime.Logic.Components;

namespace Runtime.Logic.States.Player {
    public class PlayerAttackState : IState {
        private AttackComponent _attackComponent;
        private FindTargetByDistance _targetByDistanceComponent;

        public PlayerAttackState(AttackComponent attackComponent, FindTargetByDistance targetByDistanceComponent) {
            _attackComponent = attackComponent;
            _targetByDistanceComponent = targetByDistanceComponent;
        }

        public void Enter() {
        }
        
        public void Tick() {
            _attackComponent?.Update(_targetByDistanceComponent.CurrentTarget.Transform.GetComponent<IDamagable>());
        }

        public void Exit() {
        }
    }
}