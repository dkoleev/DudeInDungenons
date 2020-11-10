using Runtime.Logic.Core.EventBus;
using UnityEngine;

namespace Runtime.Logic.Events {
    public readonly struct OnMovePerformed : IEvent {
        public readonly Vector2 Axis;

        public OnMovePerformed(Vector2 axis) {
            Axis = axis;
        }
    }
}