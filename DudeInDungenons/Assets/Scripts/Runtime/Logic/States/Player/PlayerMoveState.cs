using Avocado.Framework.Patterns.StateMachine;
using Runtime.Logic.Components;

namespace Runtime.Logic.States.Player {
    public class PlayerMoveState : IState {
        private FindTargetByDistance _targetByDistanceComponent;
        private RotateByAxis _rotator;
        private MoveByController _mover;

        public PlayerMoveState(RotateByAxis rotateByAxis, MoveByController mover, FindTargetByDistance targetByDistanceComponent) {
            _targetByDistanceComponent = targetByDistanceComponent;
            _rotator = rotateByAxis;
            _mover = mover;
        }
        
        public void Enter() {
            _targetByDistanceComponent.SetTarget(null);
        }
        
        public void Tick() {
            _rotator.Rotate(_mover.MoveAxis);
        }

        public void Exit() {
        }
    }
}