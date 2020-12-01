using Runtime.Data.Items;
using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events.Ui.Menu {
    public readonly struct OnCurrentItemChangedInShop : IEvent {
        public readonly ItemsReference.ItemType ItemType;
        public readonly Item Data;

        public OnCurrentItemChangedInShop( ItemsReference.ItemType itemType, Item data) {
            ItemType = itemType;
            Data = data;
        }
    }
}