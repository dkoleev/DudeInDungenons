using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnMovePlayer : IEvent {
        public readonly bool IsMove;

        public OnMovePlayer(bool isMove) {
            IsMove = isMove;
        }
    }
}
