using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnSpendResources : IEvent {
        public readonly string ResourceId;
        public readonly int Amount;
        
        public OnSpendResources(string resourceId, int amount) {
            ResourceId = resourceId;
            Amount = amount;
        }
    }
}
