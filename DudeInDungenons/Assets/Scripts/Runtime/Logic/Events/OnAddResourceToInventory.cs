using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnAddResourceToInventory : IEvent {
        public readonly ResourceId ResourceId;
        public readonly int Amount;

        public OnAddResourceToInventory(ResourceId resourceId, int amount) {
            ResourceId = resourceId;
            Amount = amount;
        }
    }
}