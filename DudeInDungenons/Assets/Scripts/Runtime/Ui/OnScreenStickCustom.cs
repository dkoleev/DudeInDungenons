using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Ui {
    public class OnScreenStickCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public Action<bool> OnPointerChangeState;
        
        public void OnPointerDown(PointerEventData eventData) {
            OnPointerChangeState?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData) {
            OnPointerChangeState?.Invoke(false);
        }
    }
}
