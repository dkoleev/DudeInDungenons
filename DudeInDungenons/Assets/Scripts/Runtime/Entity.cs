using System.Collections.Generic;
using Runtime.Logic.Components;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Logic.GameProgress;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime {
    public class Entity : MonoBehaviour {
        [ShowInInspector, ReadOnly]
        protected readonly List<IComponent> Components = new List<IComponent>();
        protected GameController GameController;
        protected GameProgress Progress => GameController.Progress;

        protected bool Initialized { get; private set; }

        protected virtual void Awake() {
        }

        protected virtual void Start() {
            InitializeComponents();
        }
        
        public virtual void Initialize(GameController gameController) {
            GameController = gameController;
        }

        protected void InitializeComponents() {
            if (Initialized) {
                return;
            }

            foreach (var component in Components) {
                component.Initialize();
            }
            
            EventBus<OnEntityCreated>.Raise(new OnEntityCreated(this));

            Initialized = true;
        }

        protected virtual void Update() {
        }

        protected void AddComponent(IComponent component) {
            Components.Add(component);
        }
    }
}
