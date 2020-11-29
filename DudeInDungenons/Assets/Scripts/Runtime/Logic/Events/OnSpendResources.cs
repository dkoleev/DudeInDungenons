using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnSpendResources : IEvent {
        public readonly ResourceId ResourceId;
        public readonly int Amount;
        
        public OnSpendResources(ResourceId resourceId, int amount) {
            ResourceId = resourceId;
            Amount = amount;
        }
    }
}
