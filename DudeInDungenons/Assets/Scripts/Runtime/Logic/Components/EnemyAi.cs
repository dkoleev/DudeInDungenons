namespace Runtime.Logic.Components {
    public class EnemyAi {
        public bool IsAttack { get; private set; }
        
        private Enemy _enemy;
        private Player _player;

        public EnemyAi(Enemy enemy, Player player) {
            _enemy = enemy;
            _player = player;
        }

        public void Update() {
            _enemy.NavMeshAgent.destination = _player.MainTransform.position;
            Attack(IsTargetReached());
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
    }
}