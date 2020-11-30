using Runtime.Data;
using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events.Ui.Menu {
    public readonly struct OnCurrentPetChangedInShop : IEvent {
        public readonly PetData PetData;

        public OnCurrentPetChangedInShop(PetData petData) {
            PetData = petData;
        }
    }
}