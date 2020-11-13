using System;
using System.Collections;
using Runtime.Input;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using Runtime.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime {
    public enum GameMode {
        MainMenu,
        Level
    }
    
    public class GameController : MonoBehaviour {
        private enum RunMode {
            MainMenu,
            Level
        }

        [SerializeField]
        private RunMode _runMode;

        [SerializeField]
        [ShowIf("_runMode", RunMode.Level)]
        private string _levelToLoad;

        [SerializeField, Required]
        private ItemsReference _itemsReference;
        [SerializeField, Required]
        private UiManager _uiManager;
        
        private SaveEngine<GameProgress> _saveEngine;
        private InputManager _inputManager;
        private Player _player;
        private GameProgress _progress;
        private bool _allScenesLoaded;
        private Action OnAllScenesLoaded;
        
        private GameMode _gameMode = GameMode.MainMenu;

        private void Awake() {
            _progress = LoadGameProgress();
            _inputManager = new InputManager();

            if (_runMode == RunMode.MainMenu) {
                LoadMainMenu();
            } else {
                LoadLevel(_levelToLoad);
            }
        }

        private void Start() {
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
                        _uiManager.MainMenu.OnPlayClick.AddListener(() => {
                            LoadLevel("Level_0");
                        });
                        break;
                    case GameMode.Level:
                        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
                        _player.Initialize(_progress);
                        break;
                }
            }
        }

        private GameProgress LoadGameProgress() {
            _saveEngine = new SaveEngine<GameProgress>(new GameProgress());
            return _saveEngine.LoadProgress();
        }

        private void LoadLevel(string levelName) {
            _gameMode = GameMode.Level;
            StartCoroutine(LoadLevelCor(levelName));
        }

        private void LoadMainMenu() {
            _gameMode = GameMode.MainMenu;
            StartCoroutine(LoadMainMenuCor());
        }
        
        private IEnumerator LoadMainMenuCor() {
            yield return StartCoroutine(UnloadScene(SceneNames.LevelUI));
            yield return StartCoroutine(UnloadScene("Level_0"));
            yield return StartCoroutine(LoadScene(SceneNames.MenuMain, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(SceneNames.MenuUI, LoadSceneMode.Additive));
            
            _allScenesLoaded = true;
            OnAllScenesLoaded?.Invoke();
        }
        
        private IEnumerator LoadLevelCor(string levelName) {
            yield return StartCoroutine(UnloadScene(SceneNames.MenuMain));
            yield return StartCoroutine(UnloadScene(SceneNames.MenuUI));
            
            yield return StartCoroutine(LoadScene(SceneNames.LevelUI, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(levelName, LoadSceneMode.Additive));

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
    }
}