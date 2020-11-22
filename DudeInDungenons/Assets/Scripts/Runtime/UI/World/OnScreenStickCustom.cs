using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Ui.World {
    public class OnScreenStickCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public void OnPointerDown(PointerEventData eventData) {
            EventBus<OnMovePlayer>.Raise(new OnMovePlayer(true));
        }

        public void OnPointerUp(PointerEventData eventData) {
            EventBus<OnMovePlayer>.Raise(new OnMovePlayer( false));
        }
    }
}
