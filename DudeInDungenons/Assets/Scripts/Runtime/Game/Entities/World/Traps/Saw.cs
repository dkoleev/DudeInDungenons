using System;
using UnityEngine;

namespace Runtime.Game.Entities.World.Traps {
    public class Saw : MonoBehaviour {
        public Action<Collider> OnTriggerEnterAction;
        private void OnTriggerEnter(Collider other) {
            OnTriggerEnterAction?.Invoke(other);
        }
    }
}