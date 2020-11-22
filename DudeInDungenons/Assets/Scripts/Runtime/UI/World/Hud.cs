using System;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Ui.World.Windows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Ui.World {
    public class Hud : MonoBehaviour,
            IEventReceiver<OnPlayerDead> {
        
        [SerializeField, Required]
        private Button _pauseButton;
        [SerializeField, Required]
        private GameObject _stick;
        [SerializeField, Required]
        private PauseWindow _pauseWindow;
        [SerializeField, Required]
        private LoseWindow _loseWindow;
        
        private OnScreenStickCustom _mover;
        private void Awake() {
            EventBus.Register(this);
            
            _mover = GetComponentInChildren<OnScreenStickCustom>();
            
            _pauseButton.onClick.AddListener(ShowPauseMenu);
            _pauseWindow.OnShow.AddListener(HideStick);
            _pauseWindow.OnHide.AddListener(ShowStick);
            _loseWindow.OnShow.AddListener(HideStick);
            _loseWindow.OnHide.AddListener(ShowStick);
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

        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}