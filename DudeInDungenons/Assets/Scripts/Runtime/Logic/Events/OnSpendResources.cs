using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnSpendResources : IEvent {
        public readonly ItemStack Item;
        
        public OnSpendResources(ItemStack item) {
            Item = item;
        }
    }
}
