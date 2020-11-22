using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using Runtime.Data;
using Runtime.Input;
using Runtime.Logic;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using Runtime.Static;
using Runtime.UI;
using Runtime.UI.MainMenu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime {
    public enum GameMode {
        MainMenu,
        Level
    }
    
    public class GameController : MonoBehaviour {
        private enum RunMode {
            MainMenu,
            Level,
            Empty
        }

        [SerializeField]
        private RunMode _runMode;

        [SerializeField]
        [ShowIf("_runMode", RunMode.Level)]
        private string _levelToLoad;
        [SerializeField]
        [ShowIf("_runMode", RunMode.Level)]
        private WorldData _worldData;

        [SerializeField, Required]
        private ItemsReference _itemsReference;
        [SerializeField, Required]
        private UiManager _uiManager;
        
        public WorldData CurrentWorldData { get; private set; }
        
        private SaveEngine<GameProgress> _saveEngine;
        private InputManager _inputManager;
        private Player _player;
        private GameProgress _progress;
        private bool _allScenesLoaded;
        private Action OnAllScenesLoaded;

        private Level _currentLevel;

        private GameMode _gameMode = GameMode.MainMenu;

        private void Awake() {
            _progress = LoadGameProgress();
            _inputManager = new InputManager();
            ShowLoadingScreen();

            switch (_runMode) {
                case RunMode.MainMenu:
                    LoadMainMenu(null);
                    break;
                case RunMode.Level:
                    LoadLevel(_levelToLoad);
                    break;
                case RunMode.Empty:

                    break;
            }

            AddDevCommands();
        }

        private IEnumerator Start() {
            _itemsReference.Initialize();

            if (_allScenesLoaded) {
                InitMode();
            } else {
                OnAllScenesLoaded += InitMode;
            }

            void InitMode() {
                _uiManager.Initialize(_progress, _itemsReference, _gameMode);

                switch (_gameMode) {
                    case GameMode.MainMenu:
                        CurrentWorldData = FindObjectOfType<WorldVisual>().Data;
                        _uiManager.MainMenu.OnPlayClick.AddListener(() => {
                            LoadLevel(CurrentWorldData.Levels[0].Value);
                        });
                        break;
                    case GameMode.Level:
                        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
                        _player.Initialize(_progress);
                        break;
                }
            }
            
            yield return new WaitForSeconds(1.0f);
            HideLoadingScreen();
            yield return null;
        }

        private void ShowLoadingScreen() {
            StartCoroutine(LoadScene(SceneNames.Loading, LoadSceneMode.Additive));
        }

        private void HideLoadingScreen() {
            StartCoroutine(UnloadScene(SceneNames.Loading));
        }

        private GameProgress LoadGameProgress() {
            _saveEngine = new SaveEngine<GameProgress>(new GameProgress());
            return _saveEngine.LoadProgress();
        }

        public void LoadLevel(string levelName) {
            _gameMode = GameMode.Level;
            StartCoroutine(LoadLevelCor(levelName));
        }

        public void LoadMainMenu(string levelToUnloadName) {
            _gameMode = GameMode.MainMenu;
            StartCoroutine(LoadMainMenuCor(levelToUnloadName));
        }
        
        private IEnumerator LoadMainMenuCor(string currentLevel) {
            yield return StartCoroutine(UnloadScene(SceneNames.LevelUI));
            if (!string.IsNullOrEmpty(currentLevel)) {
                yield return StartCoroutine(UnloadScene(currentLevel));
            }

            yield return StartCoroutine(LoadScene(SceneNames.MenuMain, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(SceneNames.MenuUI, LoadSceneMode.Additive));
            
            _allScenesLoaded = true;
            OnAllScenesLoaded?.Invoke();
        }
        
        private IEnumerator LoadLevelCor(string levelName) {
            yield return StartCoroutine(LoadScene(SceneNames.Loading, LoadSceneMode.Additive));
            
            if (_currentLevel != null) {
                yield return StartCoroutine(UnloadScene(_currentLevel.LevelName));
            }

            yield return StartCoroutine(UnloadScene(SceneNames.MenuMain));
            yield return StartCoroutine(UnloadScene(SceneNames.MenuUI));
            
            yield return StartCoroutine(LoadScene(SceneNames.LevelUI, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(levelName, LoadSceneMode.Additive));

            _currentLevel = new Level(this, levelName);
            
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(UnloadScene(SceneNames.Loading));
            
            _allScenesLoaded = true;
            OnAllScenesLoaded?.Invoke();
        }

        private IEnumerator LoadScene(string sceneName, LoadSceneMode mode) {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded) {
                var asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
                while (!asyncLoad.isDone) {
                    yield return null;
                }
            }

            yield return null;
        }

        private IEnumerator UnloadScene(string sceneName) {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) {
                var asyncLoad = SceneManager.UnloadSceneAsync(sceneName);
                while (!asyncLoad.isDone) {
                    yield return null;
                }
            }

            yield return null;
        }
        
        private void OnApplicationQuit() {
            _saveEngine.SaveProgress();
        }

        private void AddDevCommands() {
            DeveloperConsole.Instance.AddCommand(new DevCommand("spawn", "Spawn object on scene",
                delegate(string entityId) {
                    Addressables.InstantiateAsync(entityId, Vector3.zero, Quaternion.identity);
                }));

            DeveloperConsole.Instance.AddCommand(new DevCommand("add",
                "Add resource to inventory: add [resource_id] [amount]"
                , delegate(List<string> list) {
                    var ids = Enum.GetNames(typeof(ResourceId)).ToList();
                    if (!ids.Contains(list[0])) {
                        Debug.LogError("not found resource with id " + list[0]);
                        return;
                    }
                    
                    var enumId = (ResourceId) Enum.Parse(typeof(ResourceId), list[0]);

                    _progress.Player.AddResource(enumId, Int32.Parse(list[1]));
                }));
        }
    }
}