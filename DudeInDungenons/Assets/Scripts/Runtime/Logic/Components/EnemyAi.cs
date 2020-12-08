using Runtime.Game.Entities.Enemy;

namespace Runtime.Logic.Components {
    public class EnemyAi : IComponent {
        public bool IsAttack { get; private set; }
        public bool IsTargetReached { get; private set; }
        
        private Enemy _enemy;
        private ILocalPositionAdapter _target;

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

        public void Update(bool takeDamage) {
            _enemy.NavMeshAgent.destination = _target.LocalPosition;

            IsTargetReached = TargetReached();
            Attack(IsTargetReached && !takeDamage);
            SetCanMove(IsTargetReached || takeDamage);
        }
        
        private bool TargetReached() {
            if (!_enemy.NavMeshAgent.pathPending) {
                if (_enemy.NavMeshAgent.remainingDistance <= _enemy.NavMeshAgent.stoppingDistance) {
                    if (!_enemy.NavMeshAgent.hasPath || _enemy.NavMeshAgent.velocity.sqrMagnitude <= 0f) {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetCanMove(bool canMove) {
            _enemy.NavMeshAgent.isStopped = canMove;
        }

        private void Attack(bool canAttack) {
            IsAttack = canAttack;               
        }
    }
}