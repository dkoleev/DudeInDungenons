using Runtime.Ui.World;
using UnityEngine;

namespace Runtime.UI {
    public class UiManager : MonoBehaviour {
        public MainMenu.MainMenu MainMenu => _mainMenu;
        private Hud _hud;
        private MainMenu.MainMenu _mainMenu;
        
        public void Initialize(GameController gameController, ItemsReference itemsReference, GameMode gameMode) {
            switch (gameMode) {
                case GameMode.MainMenu:
                    _mainMenu = FindObjectOfType<MainMenu.MainMenu>();
                    if (!_mainMenu.Initialized) {
                        _mainMenu.Initialize(gameController, itemsReference);
                    }
                    break;
                case GameMode.Level:
                    _hud = FindObjectOfType<Hud>();
                    if (!_hud.Initialized) {
                        _hud.Initialize(gameController, itemsReference);
                    }
                    break;
            }
        }
    }
}