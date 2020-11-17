using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnSoulCreated : IEvent {
        public readonly Soul Soul;

        public OnSoulCreated(Soul soul) {
            Soul = soul;
        }
    }
}