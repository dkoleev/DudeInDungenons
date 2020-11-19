using System.Collections.Generic;
using System.Linq;
using Runtime.Data;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.Events.Ui;
using Object = UnityEngine.Object;

namespace Runtime {
    public class Level : 
        IEventReceiver<OnSoulCreated>,
        IEventReceiver<OnExitLevelClick>,
        IEventReceiver<OnContinueLevelClick> {
        public string LevelName { get; private set; }

        private GameController _gameController;
        private List<Enemy> _allEnemies = new List<Enemy>();
        private bool _levelCleaned;
        private int _deadEnemiesCount;
        private WorldData _worldData;
        private Portal _portal;
        private Player _player;
        private List<Soul> _souls;
         
        public Level(GameController gameController, string levelName) {
            _gameController = gameController;
            _worldData = _gameController.CurrentWorldData;
            LevelName = levelName;
            
            EventBus.Register(this);
            
            _souls = new List<Soul>();

            _portal = Object.FindObjectOfType<Portal>();
            _portal.SetContent(this);

            _player = Object.FindObjectOfType<Player>();
            
            _allEnemies = Object.FindObjectsOfType<Enemy>().ToList();
            
            foreach (var enemy in _allEnemies) {
                enemy.OnDead.AddOnce(() => {
                    _deadEnemiesCount++;
                });
            }
        }

        private void ActivatePortal() {
            _portal.Activate();
        }

        private void CollectSouls() {
            foreach (var soul in _souls) {
                soul.MoveTo(_player.MainTransform);
            }
            _souls.Clear();
        }

        public void LoadNextLevel() {
            var myIndex = GetCurrentLevelIndex();
            if (myIndex < _worldData.Levels.Count-1) {
                _gameController.LoadLevel(_worldData.Levels[myIndex+1].Value);
            } else {
                _gameController.LoadMainMenu(_worldData.Levels[myIndex].Value);
            }
        }

        private int GetCurrentLevelIndex() {
            return  _worldData.Levels.FindIndex(0, _worldData.Levels.Count, value => value.Value == LevelName);
        }

        private void Resurrect() {
            _player.Resurrect();
        }

        public void OnEvent(OnSoulCreated e) {
            _souls.Add(e.Soul);
            _levelCleaned = _souls.Count == _allEnemies.Count;

            if (_levelCleaned) {
                ActivatePortal();
                CollectSouls();
                Dispose();
            }
        }
        
        public void OnEvent(OnExitLevelClick e) {
            _gameController.LoadMainMenu(LevelName);
        }
        
        public void OnEvent(OnContinueLevelClick e) {
            Resurrect();
        }

        private void Dispose() {
            EventBus.UnRegister(this);
        }
    }
}