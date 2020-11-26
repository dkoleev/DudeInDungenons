using System.Collections.Generic;
using System.Linq;
using Runtime.Data;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.Events.Ui;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Logic {
    public class Stage : 
        IEventReceiver<OnSoulCreated>,
        IEventReceiver<OnExitLevelClick>,
        IEventReceiver<OnContinueLevelClick>,
        IEventReceiver<OnRewardCollected> {
        public string LevelName { get; private set; }

        private GameController _gameController;
        private List<Enemy> _allEnemies = new List<Enemy>();
        private bool _levelCleaned;
        private int _deadEnemiesCount;
        private WorldData _worldData;
        private Portal _portal;
        private Player _player;
        private List<Soul> _souls;
         
        public Stage(string levelName, GameController gameController, Player player) {
            _gameController = gameController;
            _worldData = _gameController.CurrentWorldData;
            LevelName = levelName;
            _player = player;

            EventBus.Register(this);
            
            _souls = new List<Soul>();

            _portal = Object.FindObjectOfType<Portal>();
            _portal.SetContent(this);
            
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
            if (IsLastStage()) {
                EventBus<OnLevelCompleted>.Raise(new OnLevelCompleted());
            } else {
                Dispose();
                _gameController.LoadLevel(_worldData.Levels[myIndex+1].Value);
            }
        }

        private bool IsLastStage() {
            return GetCurrentLevelIndex() >= _worldData.Levels.Count - 1;
        }

        private int GetCurrentLevelIndex() {
            return  _worldData.Levels.FindIndex(0, _worldData.Levels.Count, value => value.Value == LevelName);
        }

        private void Resurrect() {
            _player.Resurrect();
        }

        public void LoadMainMenu() {
            Dispose();
            _gameController.LoadMainMenu(LevelName);
        }

        public void OnEvent(OnSoulCreated e) {
            _souls.Add(e.Soul);
            _levelCleaned = _souls.Count == _allEnemies.Count;

            if (_levelCleaned) {
                ActivatePortal();
                CollectSouls();
            }
        }
        
        public void OnEvent(OnExitLevelClick e) {
            EventBus<OnLevelCompleted>.Raise(new OnLevelCompleted());
        }
        
        public void OnEvent(OnContinueLevelClick e) {
            Resurrect();
        }
        
        public void OnEvent(OnRewardCollected e) {
            LoadMainMenu();
        }

        public void Dispose() {
            EventBus.UnRegister(this);
        }
    }
}