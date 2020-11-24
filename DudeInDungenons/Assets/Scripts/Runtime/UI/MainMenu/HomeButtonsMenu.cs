using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.UI.MainMenu {
    public class HomeButtonsMenu : MonoBehaviour {
        [SerializeField, Required]
        private List<HomeButton> _buttons;

        private MainMenu _menu;
        
        public void Initialize(MainMenu mainMenu) {
            _menu = mainMenu;
            
            foreach (var button in _buttons) {
                button.Initialize();
                button.SetActive(button.Category == MainMenu.MenuCategory.World, false);
                button.OnClick.AddListener(OnClickButton);
            }
        }

        private void OnClickButton(MainMenu.MenuCategory category) {
            _menu.SelectCategory(category);
            SelectButton(category);
        }

        private void SelectButton(MainMenu.MenuCategory category) {
            foreach (var button in _buttons) {
                button.SetActive(button.Category == category);
            }
        }

        private void OnDestroy() {
            foreach (var button in _buttons) {
                button.OnClick.RemoveAll();
            }
        }
    }
}