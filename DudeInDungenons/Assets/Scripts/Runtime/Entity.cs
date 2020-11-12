using System.Collections.Generic;
using Runtime.Logic.Components;
using UnityEngine;

namespace Runtime {
    public class Entity : MonoBehaviour {
        protected readonly List<IComponent> Components = new List<IComponent>();
        
        protected virtual void Awake() {
        }

        protected virtual void Start() {
            foreach (var component in Components) {
                component.Initialize();
            }
        }

        protected virtual void Update() {
        }

        protected void AddComponent(IComponent component) {
            Components.Add(component);
        }
    }
}
