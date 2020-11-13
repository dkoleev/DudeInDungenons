using System.Collections;
using Runtime.Input;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using Runtime.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime {
    public class GameController : MonoBehaviour {
        public enum RunMode {
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

        private void Awake() {
            _progress = LoadGameProgress();
            _inputManager = new InputManager();

            LoadMainMenu();
        }

        private void Start() {
            _itemsReference.Initialize();
            _uiManager.Initialize(_progress, _itemsReference);

            _uiManager.MainMenu.OnPlayClick.AddListener(LoadLevel);
        }

        private GameProgress LoadGameProgress() {
            _saveEngine = new SaveEngine<GameProgress>(new GameProgress());
            return _saveEngine.LoadProgress();
        }

        private void LoadLevel() {
            StartCoroutine(LoadLevelCor());
        }

        private void LoadMainMenu() {
            StartCoroutine(LoadMainMenuCor());
        }
        
        private IEnumerator LoadMainMenuCor() {
            yield return StartCoroutine(UnloadScene(SceneNames.LevelMain));
            yield return StartCoroutine(UnloadScene(SceneNames.LevelUI));
            yield return StartCoroutine(UnloadScene("Level_0"));
            yield return StartCoroutine(LoadScene(SceneNames.MenuMain, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(SceneNames.MenuUI, LoadSceneMode.Additive));
        }
        
        private IEnumerator LoadLevelCor() {
            yield return StartCoroutine(UnloadScene(SceneNames.MenuMain));
            yield return StartCoroutine(UnloadScene(SceneNames.MenuUI));
            
            yield return StartCoroutine(LoadScene(SceneNames.LevelMain, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(SceneNames.LevelUI, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene("Level_0", LoadSceneMode.Additive));

            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            _player.Initialize(_progress);
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