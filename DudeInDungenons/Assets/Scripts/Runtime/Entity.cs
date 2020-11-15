using System.Collections.Generic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.GameProgress;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class Entity : MonoBehaviour {
        [ShowInInspector]
        protected readonly List<IComponent> Components = new List<IComponent>();
        protected GameProgress Progress;

        protected virtual void Awake() {
        }

        protected virtual void Start() {
            foreach (var component in Components) {
                component.Initialize();
            }
            
            EventBus<OnEntityCreated>.Raise(new OnEntityCreated(this));
        }
        
        public void Initialize(GameProgress progress) {
            Progress = progress;
        }

        protected virtual void Update() {
        }

        protected void AddComponent(IComponent component) {
            Components.Add(component);
        }
    }
}
