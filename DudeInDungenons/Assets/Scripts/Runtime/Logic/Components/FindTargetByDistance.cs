using System.Collections.Generic;
using System.Linq;
using Runtime.Static;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class FindTargetByDistance : IComponent {
        public Transform CurrentTarget => _currentTarget;

        private Transform _owner;
        private Transform _currentTarget;
        private List<Transform> _targets = new List<Transform>();
        private EntityTag _targetTag;

        public FindTargetByDistance(Transform owner, EntityTag targetTag) {
            _owner = owner;
            _targetTag = targetTag;
        }
        
        public void Initialize() {
            var objects = GameObject.FindGameObjectsWithTag(_targetTag.ToString()).ToList();
            foreach (var go in objects) {
                _targets.Add(go.transform);
            }
        }

        public void Update() {
            SetTargetByDistance();
        }

        public void SetTarget(Transform target) {
            _currentTarget = target;
        }

        private void SetTargetByDistance() {
            if (_currentTarget == null) {
                foreach (var target in _targets) {
                    if (target == null) {
                        continue;
                    }

                    if (_currentTarget == null) {
                        _currentTarget = target;
                    } else if(target != _currentTarget &&
                              Vector3.Distance(_owner.position, target.localPosition) < Vector3.Distance(_owner.position, _currentTarget.localPosition)) {
                        _currentTarget = target;
                    }
                }
            }
        }
    }
}
