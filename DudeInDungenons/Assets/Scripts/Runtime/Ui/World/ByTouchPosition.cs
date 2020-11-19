using System;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events.Ui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Ui.World {
    public class ByTouchPosition : MonoBehaviour, IEventReceiver<OnClick> {
        [SerializeField]
        private Vector2 _offset;

        private Vector3 _bufferPosition;

        private void Awake() {
            _bufferPosition = transform.position;
            EventBus.Register(this);
        }

        private void Update() {
#if UNITY_EDITOR
            if (!Mouse.current.leftButton.IsPressed()) {
                transform.position = _bufferPosition;
            }
#else
             if (!Touchscreen.current.IsPressed()) {
                transform.position = _bufferPosition;
            }
#endif
        }

        public void OnEvent(OnClick e) {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
#if UNITY_EDITOR
            transform.position = new Vector3(Mouse.current.position.x.ReadValue() + _offset.x,
                Mouse.current.position.y.ReadValue() + _offset.y, transform.position.z);
#else
            transform.position =
 new Vector3( Touchscreen.current.position.x.ReadValue() + _offset.x,  Touchscreen.current.position.y.ReadValue() + _offset.y, transform.position.z);
#endif
        }

        private void OnDestroy() {
            EventBus.UnRegister(this);
        }
    }
}