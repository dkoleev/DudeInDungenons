using System;
using System.Collections;
using System.Collections.Generic;
using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using Runtime.Data;
using Runtime.Input;
using Runtime.LocalNotifications;
using Runtime.Logic;
using Runtime.Logic.Converters;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using Runtime.Logic.GameProgress.Progress;
using Runtime.Logic.Managers;
using Runtime.Static;
using Runtime.UI;
using Runtime.UI.MainMenu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Inventory = Runtime.Logic.Inventory.Inventory;

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
        private SettingsReference _settingsReference;
        [SerializeField, Required]
        private UiManager _uiManager;
        [SerializeField, Required]
        private BillingManager _billingManager;
        [SerializeField, Required]
        private GameNotificationsManager _localNotificationsManager;

        [SerializeField, Required]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
        public ItemsReference ItemReference => _itemsReference;

        public WorldData CurrentWorldData { get; private set; }
        public World СurrentWorld => _currentWorld;
        public WorldVisual CurrentWorldVisual { get; private set; }
        public SettingsReference SettingsReference => _settingsReference;
        public Inventory Inventory => _inventory;
        public BillingManager Billing => _billingManager;
        public Player Player => _player;
        public GameProgress Progress => _progress;
        
        private SaveEngine<GameProgress> _saveEngine;
        private InputManager _inputManager;
        private Player _player;
        private Inventory _inventory;
        private GameProgress _progress;
        private bool _allScenesLoaded;
        private Action OnAllScenesLoaded;

        private World _currentWorld;
        private LocalNotificationsRegistration _notificationsRegistration;
        private List<ManagerBase> _managers = new List<ManagerBase>();

        private GameMode _gameMode = GameMode.MainMenu;
        private bool _initialized;

        private void Awake() {
            Application.targetFrameRate = 60;
            
            _progress = LoadGameProgress();
            PutStartProgress();
          
            _inventory = new Inventory(_progress);
            _inputManager = new InputManager();
            InitializeManagers();
            _notificationsRegistration = new LocalNotificationsRegistration(_localNotificationsManager);
            
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

        private void InitializeManagers() {
            var resourceConverter = new ResourceConvertManager(this);
            
            _managers.Add(resourceConverter);
        }

        private void PutStartProgress() {
            if (!_progress.FirstRun) {
                return;
            }

            _progress.FirstRun = false;
            if (string.IsNullOrEmpty(_progress.Player.CurrentSkin)) {
                _progress.Player.CurrentSkin = _playerData.StartSkin.Id;
            }

            foreach (var itemStack in _playerData.StartInventory) {
                if (!_progress.Player.Inventory.ContainsKey(itemStack.Item.Id)) {
                    _progress.Player.Inventory.Add(itemStack.Item.Id, new ItemProgress(itemStack.Item.Id, itemStack.Amount));
                }
            }
        }

        private IEnumerator Start() {
            _itemsReference.Initialize();
            AdsManager.Instance.Initialize();
            _billingManager.Initialize(this);

            if (_allScenesLoaded) {
                InitMode();
            } else {
                OnAllScenesLoaded += InitMode;
            }

            void InitMode() {
                switch (_gameMode) {
                    case GameMode.MainMenu:
                        CurrentWorldVisual = FindObjectOfType<WorldVisual>();
                        CurrentWorldData = CurrentWorldVisual.Data;
                        _uiManager.Initialize(this, _itemsReference, _gameMode);
                        _uiManager.MainMenu.OnPlayClick.AddListener(LoadWorld);
                        break;
                    case GameMode.Level:
                        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
                        _player.Initialize(this);
                        _uiManager.Initialize(this, _itemsReference, _gameMode);
                        break;
                }
                
            }
            
            _initialized = true;
            
            yield return new WaitForSeconds(1.0f);
            HideLoadingScreen();
            yield return null;
        }

        private void Update() {
            if (!_initialized) {
                return;
            }

            foreach (var manager in _managers) {
                manager.Update();
            }
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

        private void LoadWorld() {
            LoadLevel(CurrentWorldData.Levels[0].Value, true);
        }

        public void LoadLevel(string levelName, bool isFirstStage = false) {
            _gameMode = GameMode.Level;
            StartCoroutine(LoadLevelCor(levelName, isFirstStage));
        }

        public void LoadMainMenu(string levelToUnloadName) {
            _gameMode = GameMode.MainMenu;
            StartCoroutine(LoadMainMenuCor(levelToUnloadName));
        }

        private IEnumerator LoadMainMenuCor(string currentLevel) {
            yield return StartCoroutine(LoadScene(SceneNames.Loading, LoadSceneMode.Additive));
            
            yield return new WaitForSeconds(0.5f);
            
            yield return StartCoroutine(UnloadScene(SceneNames.WorldUI));
            if (!string.IsNullOrEmpty(currentLevel)) {
                yield return StartCoroutine(UnloadScene(currentLevel));
            }
            yield return StartCoroutine(UnloadScene(SceneNames.WorldBase));

            yield return StartCoroutine(LoadScene(SceneNames.MenuUI, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(SceneNames.MenuWorld, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(SceneNames.MenuHero, LoadSceneMode.Additive));
            
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(UnloadScene(SceneNames.Loading));
            
            _allScenesLoaded = true;
            OnAllScenesLoaded?.Invoke();
        }
        
        private IEnumerator LoadLevelCor(string stageToLoad, bool isFirstStage = false) {
            yield return StartCoroutine(LoadScene(SceneNames.Loading, LoadSceneMode.Additive));
            
            yield return StartCoroutine(UnloadScene(SceneNames.MenuWorld));
            yield return StartCoroutine(UnloadScene(SceneNames.MenuHero));
            yield return StartCoroutine(UnloadScene(SceneNames.MenuUI));
            
            if (isFirstStage) {
                yield return StartCoroutine(LoadScene(SceneNames.WorldBase, LoadSceneMode.Additive));
                _currentWorld = new World(this);
            } else {
                yield return StartCoroutine(UnloadScene(_currentWorld.CurrentStage.LevelName));
                
                _currentWorld.DisposeStage();
                _currentWorld.StartSetup();
            }
            
            yield return StartCoroutine(LoadScene(SceneNames.WorldUI, LoadSceneMode.Additive));
            yield return StartCoroutine(LoadScene(stageToLoad, LoadSceneMode.Additive));
            
            _currentWorld.Enter(stageToLoad);
            
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(UnloadScene(SceneNames.Loading));
            
            _allScenesLoaded = true;
            OnAllScenesLoaded?.Invoke();
        }

        private IEnumerator LoadScene(string sceneName, LoadSceneMode mode) {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded) {
                yield return SceneManager.LoadSceneAsync(sceneName, mode);
            }

            yield return null;
        }

        private IEnumerator UnloadScene(string sceneName) {
            if (SceneManager.GetSceneByName(sceneName).isLoaded) {
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }

            yield return null;
        }

        private void OnDestroy() {
            foreach (var manager in _managers) {
                manager.Dispose();
            }
        }

        private void OnApplicationQuit() {
            _saveEngine.SaveProgress();
        }

        private void AddDevCommands() {
            DeveloperConsole.Instance.AddCommand(new DevCommand("spawn", "Spawn object on scene",
                delegate(string entityId) {
                    Addressables.InstantiateAsync(entityId, Vector3.zero, Quaternion.identity);
                }));

            /*DeveloperConsole.Instance.AddCommand(new DevCommand("add",
                "Add resource to inventory: add [resource_id] [amount]"
                , delegate(List<string> list) {
                    var ids = Enum.GetNames(typeof(ResourceId)).ToList();
                    if (!ids.Contains(list[0])) {
                        Debug.LogError("not found resource with id " + list[0]);
                        return;
                    }
                    
                    var enumId = (ResourceId) Enum.Parse(typeof(ResourceId), list[0]);

                    Inventory.AddResource(enumId, Int32.Parse(list[1]));
                }));*/
        }
        
        private int playReminderHour = 6;
        public void OnPlayReminder()
        {
            // Schedule a reminder to play the game. Schedule it for the next day.
            DateTime deliveryTime = DateTime.Now.ToLocalTime().AddDays(1);
            deliveryTime = new DateTime(deliveryTime.Year, deliveryTime.Month, deliveryTime.Day, playReminderHour, 0, 0,
                DateTimeKind.Local);

            _notificationsRegistration.SendNotification("Cookie Reminder", "Remember to make more cookies!", deliveryTime,
                channelId: LocalNotificationsRegistration.ReminderChannelId);
        }

        public void SendNotif() {
            DateTime deliveryTime = DateTime.Now.ToLocalTime() + TimeSpan.FromMinutes(1);
            _notificationsRegistration.SendNotification("Test", "hello world", deliveryTime, reschedule: true,
                smallIcon: "icon_0", largeIcon: "icon_1");
        }
    }
}