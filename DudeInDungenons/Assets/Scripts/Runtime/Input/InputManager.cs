using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.Events.Ui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Input {
    public class InputManager {
        public Controls MainControl { get; }

        public InputManager() {
            MainControl = new Controls();
            MainControl.Player.Move.performed += MoveOnPerformed;
            MainControl.Player.Move.canceled += MoveOnCanceled;
            
            MainControl.UI.Click.performed += ClickOnPerformed;
            
            MainControl.Enable();
        }

        private void ClickOnPerformed(InputAction.CallbackContext context) {
            EventBus<OnClick>.Raise(new OnClick());
        }

        private void MoveOnCanceled(InputAction.CallbackContext context) {
            EventBus<OnMoveCancelled>.Raise(new OnMoveCancelled());
        }

        private void MoveOnPerformed(InputAction.CallbackContext context) {
            var axis = context.ReadValue<Vector2>();
            EventBus<OnMovePerformed>.Raise(new OnMovePerformed(axis));
        }
    }
}