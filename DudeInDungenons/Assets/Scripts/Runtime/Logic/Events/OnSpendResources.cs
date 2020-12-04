using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnSpendResources : IEvent {
        public readonly Item Item;
        public readonly int Amount;
        
        public OnSpendResources(Item item, int amount) {
            Item = item;
            Amount = amount;
        }
    }
}
