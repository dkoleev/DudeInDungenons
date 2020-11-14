using UnityEngine;

namespace Runtime.Logic.Components {
    public class EnemyAi : IComponent {
        public bool IsAttack { get; private set; }
        
        private Enemy _enemy;
        private ILocalPositionAdapter _target;
        private Animator _animator;
        
        private static readonly int _animationWalk = Animator.StringToHash("Walk");
        private static readonly int _animationRun = Animator.StringToHash("Run");
        private static readonly int _animationIdle = Animator.StringToHash("Idle");
        private static readonly int _animationAttack = Animator.StringToHash("Attack");

        public EnemyAi(Enemy enemy) {
            _enemy = enemy;
        }

        public void SetTarget(ILocalPositionAdapter target) {
            _target = target;
        }

        public void Initialize() {
            _enemy.NavMeshAgent.speed = _enemy.Data.SpeedMove;
            _enemy.NavMeshAgent.angularSpeed = _enemy.Data.SpeedRotate;
        }

        public void Update() {
            _enemy.NavMeshAgent.destination = _target.LocalPosition;

            var targetReached = IsTargetReached();
            Attack(targetReached);
            SetAnimation(targetReached);
        }
        
        private bool IsTargetReached() {
            if (!_enemy.NavMeshAgent.pathPending) {
                if (_enemy.NavMeshAgent.remainingDistance <= _enemy.NavMeshAgent.stoppingDistance) {
                    if (!_enemy.NavMeshAgent.hasPath || _enemy.NavMeshAgent.velocity.sqrMagnitude <= 0f) {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Attack(bool isAttack) {
            IsAttack = isAttack;               
            _enemy.NavMeshAgent.isStopped = isAttack;
        }

        private void SetAnimation(bool isTargetReached) {
            if (_enemy.Animator == null) {
                return;
            }

            if (isTargetReached) {
                _enemy.Animator.SetTrigger(_animationAttack);
            }

            _enemy.Animator.SetBool(_animationRun, !_enemy.NavMeshAgent.isStopped);
        }
    }
}