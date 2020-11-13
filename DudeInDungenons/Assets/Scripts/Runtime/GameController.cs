using Runtime.Input;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using Runtime.Ui;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class GameController : MonoBehaviour {
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
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        private void Start() {
            _itemsReference.Initialize();
            _uiManager.Initialize(_progress, _itemsReference);
            _player.Initialize(_progress);
        }

        private GameProgress LoadGameProgress() {
            _saveEngine = new SaveEngine<GameProgress>(new GameProgress());
            return _saveEngine.LoadProgress();
        }

        private void OnApplicationQuit() {
            _saveEngine.SaveProgress();
        }
    }
}