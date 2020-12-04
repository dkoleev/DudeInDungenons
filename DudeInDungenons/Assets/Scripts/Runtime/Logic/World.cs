using UnityEngine;

namespace Runtime.Logic {
    public class World {
        public Stage CurrentStage => _currentStage;

        private GameController _gameController;
        private Stage _currentStage;
        private Player _player;
        private Vector3 _playerStartPosition;
        
        public World(GameController gameController) {
            _gameController = gameController;
            
            _player = Object.FindObjectOfType<Player>();
            _playerStartPosition = _player.transform.position;
        }

        public void StartSetup() {
            _player.transform.position = _playerStartPosition;
        }

        public void Enter(string stageName) {
            _gameController.Inventory.SpendResource(_gameController.ItemReference.EnergyData, 5);
            _currentStage = new Stage(stageName, _gameController, _player);
        }

        public void MoveDropToInventory() {
            _gameController.Inventory.Add(_player.Drop);
            _player.Drop.Clear();
        }
        
        public void MoveDropToInventoryX2() {
            _gameController.Inventory.Add(_player.Drop);
            _gameController.Inventory.Add(_player.Drop);
            _player.Drop.Clear();
        }

        public void DisposeStage() {
           // _currentStage.Dispose();
            _currentStage = null;
        }
    }
}