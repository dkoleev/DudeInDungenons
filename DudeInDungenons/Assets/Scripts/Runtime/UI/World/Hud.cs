using System.Collections.Generic;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.UI.Base;
using Runtime.Ui.World.Windows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class Hud : UiBase,
            IEventReceiver<OnPlayerDead>,
            IEventReceiver<OnLevelCompleted>,
            IEventReceiver<OnLevelCanceled> {
        
        [SerializeField, Required]
        private Button _pauseButton;
        [SerializeField, Required]
        private GameObject _stick;
        [SerializeField, Required]
        private PauseWindow _pauseWindow;
        [SerializeField, Required]
        private LoseWindow _loseWindow;
        [SerializeField, Required]
        private RewardWindow _rewardWindow;
        
        private OnScreenStickCustom _mover;
        private List<WindowBase> _windows = new List<WindowBase>();
        
        private void Awake() {
            EventBus.Register(this);
            
            _mover = GetComponentInChildren<OnScreenStickCustom>();
            
            _windows.Add(_loseWindow);
            _windows.Add(_pauseWindow);
            _windows.Add(_rewardWindow);
        }

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);
            
            foreach (var window in _windows) {
                window.Initialize(GameController, ItemsReference);
                window.OnShow.AddListener(HideStick);
                window.OnHide.AddListener(ShowStick);
            }
            
            _pauseButton.onClick.AddListener(ShowPauseMenu);
            Initialized = true;
        }

        public void ShowRewardWindow() {
            _rewardWindow.Show();
        }

        private void ShowPauseMenu() {
            _pauseWindow.Show();
        }

        private void ShowStick() {
            _stick.SetActive(true);
        }

        private void HideStick() {
            _stick.SetActive(false);
        }

        public void OnEvent(OnPlayerDead e) {
            _loseWindow.Show();
        }
        
        public void OnEvent(OnLevelCompleted e) {
            ShowRewardWindow();
        }

        public void OnEvent(OnLevelCanceled e) {
            ShowRewardWindow();
        }

        private void OnDestroy() {
            foreach (var window in _windows) {
                window.OnShow.RemoveAll();
                window.OnHide.RemoveAll();
            }

            EventBus.UnRegister(this);
        }
    }
}