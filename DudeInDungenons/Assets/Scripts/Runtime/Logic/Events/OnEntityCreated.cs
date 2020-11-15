using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnEntityCreated : IEvent {
        public readonly Entity Entity;

        public OnEntityCreated(Entity entity) {
            Entity = entity;
        }
    }
}