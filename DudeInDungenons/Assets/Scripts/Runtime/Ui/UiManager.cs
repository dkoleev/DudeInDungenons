using Runtime.Logic.GameProgress;
using Runtime.Ui.World;
using UnityEngine;

namespace Runtime.Ui {
    public class UiManager : MonoBehaviour {
        public MainMenu.MainMenu MainMenu => _mainMenu;
        private Hud _hud;
        private MainMenu.MainMenu _mainMenu;

        public void Initialize(GameProgress  progress, ItemsReference itemsReference) {
            _mainMenu = FindObjectOfType<MainMenu.MainMenu>();
            _mainMenu.Initialize(progress, itemsReference);
            _hud = FindObjectOfType<Hud>();
        }
    }
}