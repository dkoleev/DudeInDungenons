using System.Collections.Generic;
using System.Linq;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;

namespace Runtime.UI.MainMenu.Equipment {
    public class PetsShop : ItemsShop {
        protected override void LoadCurrentItem() {
            var progress = GameController.Progress.Player;
            var currentPetId = progress.CurrentPet;
            if (!string.IsNullOrEmpty(currentPetId)) {
                _selectedItem = _scrollItems.First(item => item.Data.Id == currentPetId);
                
                EventBus<OnCurrentItemChangedInShop>.Raise(new OnCurrentItemChangedInShop(_type, _selectedItem.Data));
            }
        }

        protected override void UnlockItem() {
            GameController.Progress.Player.UnlockedPets.Add(_selectedItem.Data.Id);
        }

        protected override string GetCurrentItem() {
            return GameController.Progress.Player.CurrentPet;
        }

        protected override void SetCurrentItem(string id) {
            GameController.Progress.Player.CurrentPet = id;
        }

        protected override HashSet<string> GetUnlockedItems() {
            return GameController.Progress.Player.UnlockedPets;
        }
    }
}
