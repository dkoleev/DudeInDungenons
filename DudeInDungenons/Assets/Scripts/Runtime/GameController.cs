using Runtime.Input;
using Runtime.Logic.Core.SaveEngine;
using Runtime.Logic.GameProgress;
using UnityEngine;

namespace Runtime {
    public class GameController : MonoBehaviour {
        private SaveEngine<GameProgress> _saveEngine;
        private InputManager _inputManager;

        private void Awake() {
            _inputManager = new InputManager();
            var progress = LoadGameProgress();

            var player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.Initialize(progress);
        }

        private GameProgress LoadGameProgress() {
            _saveEngine = new SaveEngine<GameProgress>(new GameProgress());
            return _saveEngine.LoadProgress() as GameProgress;
        }

        private void OnApplicationQuit() {
            _saveEngine.SaveProgress();
        }
    }
}