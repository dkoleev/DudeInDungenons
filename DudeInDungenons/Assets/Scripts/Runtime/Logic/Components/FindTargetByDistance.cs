using System.Collections.Generic;
using System.Linq;
using Runtime.Static;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class FindTargetByDistance : IComponent {
        public ITarget CurrentTarget => _currentTarget;

        private Transform _owner;
        private ITarget _currentTarget;
        private List<ITarget> _targets = new List<ITarget>();
        private EntityTag _targetTag;

        public FindTargetByDistance(Transform owner, EntityTag targetTag) {
            _owner = owner;
            _targetTag = targetTag;
        }
        
        public void Initialize() {
            var objects = GameObject.FindGameObjectsWithTag(_targetTag.ToString()).ToList();
            foreach (var go in objects) {
                _targets.Add(go.GetComponent<ITarget>());
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
                foreach (var target in _targets) {
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
