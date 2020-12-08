using Runtime.Game.Entities.Enemy;
using Runtime.Logic.Core.EventBus;

namespace Runtime.Logic.Events {
    public readonly struct OnEnemyDead : IEvent {
        public readonly Enemy Enemy;

        public OnEnemyDead(Enemy enemy) {
            Enemy = enemy;
        }
    }
}