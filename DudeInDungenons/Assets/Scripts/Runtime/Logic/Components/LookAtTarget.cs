using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Logic.Components {
    public class LookAtTarget : IComponent {
        public Enemy CurrentTarget => _currentTarget;

        private Transform _owner;
        private Enemy _currentTarget;
        private List<Enemy> _targets = new List<Enemy>();

        public LookAtTarget(Transform owner) {
            _owner = owner;
        }
        
        public void Initialize() {
            _targets = Object.FindObjectsOfType<Enemy>().ToList();
        }

        public void Update() {
            SetTargetByDistance();
        }

        public void SetTarget(Enemy target) {
            _currentTarget = target;
        }

        private void SetTargetByDistance() {
            if (_currentTarget == null) {
                foreach (var enemy in _targets) {
                    if (enemy == null) {
                        continue;
                    }

                    if (_currentTarget == null) {
                        _currentTarget = enemy;
                    } else if(enemy != _currentTarget &&
                              Vector3.Distance(_owner.position, enemy.transform.position) < Vector3.Distance(_owner.position, _currentTarget.transform.position)) {
                        _currentTarget = enemy;
                    }
                }
            }
        }
    }
}
