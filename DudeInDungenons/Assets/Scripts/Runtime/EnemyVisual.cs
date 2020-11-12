using System;
using Runtime.Ui.World;

namespace Runtime {
    public class EnemyVisual : IDisposable {
        private readonly WorldBar _healthBar;
        private readonly Enemy _enemy;

        public EnemyVisual(Enemy enemy) {
            _enemy = enemy;
            _healthBar = _enemy.GetComponentInChildren<WorldBar>();
            _healthBar.Initialize(enemy.CurrentHealth, enemy.Data.MaxHealth);

            _enemy.OnHealthChanged.AddListener(HealthChangeHandle);
        }

        private void HealthChangeHandle(float newHealth) {
            _healthBar.SetProgress(newHealth);
        }

        public void Dispose() {
            _enemy.OnHealthChanged.RemoveListener(HealthChangeHandle);
            _healthBar.Dispose();
        }
    }
}