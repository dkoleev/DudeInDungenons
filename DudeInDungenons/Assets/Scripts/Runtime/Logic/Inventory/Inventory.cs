using System.Collections.Generic;

namespace Runtime.Logic.Inventory {
    public class Inventory {
        private readonly GameProgress.GameProgress _gameProgress;
        private Dictionary<ResourceId, int> _inventory;
        
        public Inventory(GameProgress.GameProgress gameProgress) {
            _inventory = new Dictionary<ResourceId, int>();
            _gameProgress = gameProgress;
            
            _inventory = _gameProgress.Player.Inventory;
        }
    }
}