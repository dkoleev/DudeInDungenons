using System.Collections.Generic;
using System.Linq;
using Runtime.Data;
using UnityEngine;

namespace Runtime {
    public class Level {
        public string LevelName { get; private set; }

        private GameController _gameController;
        private List<Enemy> _allEnemies = new List<Enemy>();
        private bool _levelCleaned;
        private int _deadEnemiesCount;
        private WorldData _worldData;
        private Portal _portal;
         
        public Level(GameController gameController, string levelName) {
            _gameController = gameController;
            _worldData = _gameController.CurrentWorldData;
            LevelName = levelName;

            _portal = Object.FindObjectOfType<Portal>();
            _portal.SetContent(this);
            
            _allEnemies = Object.FindObjectsOfType<Enemy>().ToList();
            foreach (var enemy in _allEnemies) {
                enemy.OnDead.AddOnce(() => {
                    _deadEnemiesCount++;
                    _levelCleaned = _deadEnemiesCount == _allEnemies.Count;
                    if (_levelCleaned) {
                        ActivatePortal();
                    }
                });
            }
        }

        private void ActivatePortal() {
            _portal.Activate();
        }

        public void LoadNextLevel() {
            var myIndex = _worldData.Levels.FindIndex(0, _worldData.Levels.Count, value => value.Value == LevelName);
            if (myIndex < _worldData.Levels.Count-1) {
                _gameController.LoadLevel(_worldData.Levels[myIndex+1].Value);
            } else {
                _gameController.LoadMainMenu(_worldData.Levels[myIndex].Value);
            }
        }
    }
}