using System.Collections.Generic;
using System.Linq;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using Runtime.Static;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class FindTargetByDistance : IComponent, IEventReceiver<OnEntityCreated> {
        public ITarget CurrentTarget => _currentTarget;

        private Transform _owner;
        private ITarget _currentTarget;
        private HashSet<ITarget> _targets = new HashSet<ITarget>();
        private EntityTag _targetTag;

        public FindTargetByDistance(Transform owner, EntityTag targetTag) {
            _owner = owner;
            _targetTag = targetTag;
            EventBus.Register(this);
        }
        
        public void Initialize() {
            var objects = GameObject.FindGameObjectsWithTag(_targetTag.ToString()).ToList();
            foreach (var go in objects) {
                _targets.Add(go.GetComponent<ITarget>());
            }
        }
        
        public void OnEvent(OnEntityCreated e) {
            if (e.Entity.gameObject.CompareTag(_targetTag.ToString())) {
                _targets.Add(e.Entity as ITarget);
            }
        }

        public void Update() {
            SetTargetByDistance();
        }

        public void SetTarget(ITarget target) {
            _currentTarget = target;
        }
        
        public bool CurrentTargetIsAvailable() {
            return _currentTarget != null && _currentTarget.IsReachable;
        }
        
        private bool TargetIsAvailable(ITarget target) {
            return target != null && target.IsReachable;
        }

        private void SetTargetByDistance() {
            if (!TargetIsAvailable(_currentTarget)) {
                var bufferTargets = _targets.ToList();
                foreach (var target in bufferTargets) {
                    if (!TargetIsAvailable(target)) {
                        continue;
                    }

                    if (!TargetIsAvailable(_currentTarget)) {
                        _currentTarget = target;
                    } else if(target != _currentTarget &&
                              Vector3.Distance(_owner.position, target.Transform.localPosition) < Vector3.Distance(_owner.position, _currentTarget.Transform.localPosition)) {
                        _currentTarget = target;
                    }
                }
            }
        }
    }
}
