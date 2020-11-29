using Runtime.Logic.Core.EventBus;
using UnityEngine.AddressableAssets;

namespace Runtime.Logic.Events.Ui.Menu {
    public readonly struct OnCurrentPetChangedInShop : IEvent {
        public readonly AssetReference PetAsset;

        public OnCurrentPetChangedInShop(AssetReference petAsset) {
            PetAsset = petAsset;
        }
    }
}