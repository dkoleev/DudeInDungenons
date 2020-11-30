using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnAddResourceToInventory : IEvent {
        public readonly string ResourceId;
        public readonly int Amount;

        public OnAddResourceToInventory(string resourceId, int amount) {
            ResourceId = resourceId;
            Amount = amount;
        }
    }
}