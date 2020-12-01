using System.Collections.Generic;
using System.Linq;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui.Menu;

namespace Runtime.UI.MainMenu.Equipment {
    public class SkinsShop : ItemsShop {
        protected override void LoadCurrentItem() {
            var progress = GameController.Progress.Player;
            if (!string.IsNullOrEmpty(progress.CurrentSkin)) {
                _selectedItem = _scrollItems.First(item => item.Data.Id == progress.CurrentSkin);
                
                EventBus<OnCurrentItemChangedInShop>.Raise(new OnCurrentItemChangedInShop(_type, _selectedItem.Data));
            }
        }

        protected override void UnlockItem() {
            GameController.Progress.Player.UnlockedSkins.Add(_selectedItem.Data.Id);
        }

        protected override string GetCurrentItem() {
            return GameController.Progress.Player.CurrentSkin;
        }

        protected override void SetCurrentItem(string id) {
            GameController.Progress.Player.CurrentSkin = id;
        }

        protected override HashSet<string> GetUnlockedItems() {
            return GameController.Progress.Player.UnlockedSkins;
        }
    }
}