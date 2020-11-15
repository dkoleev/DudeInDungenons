using System;
using Runtime.Ui.World;
using UnityEngine;

namespace Runtime {
    public class EnemyVisual : IDisposable {
        private static readonly int _animationWalk = Animator.StringToHash("Walk");
        private static readonly int _animationRun = Animator.StringToHash("Run");
        private static readonly int _animationIdle = Animator.StringToHash("Idle");
        private static readonly int _animationAttack = Animator.StringToHash("Attack");
        private static readonly int _animationTakeDamage= Animator.StringToHash("TakeDamage");
        private static readonly int _animationDead= Animator.StringToHash("Dead");
        
        private readonly WorldBar _healthBar;
        private readonly Enemy _enemy;
        private Animator _animator;

        public EnemyVisual(Enemy enemy) {
            _enemy = enemy;
            _healthBar = _enemy.GetComponentInChildren<WorldBar>();
            if (_healthBar != null) {
                _healthBar.Initialize(enemy.CurrentHealth, enemy.Data.MaxHealth);
            }

            _enemy.OnHealthChanged.AddListener(HealthChangeHandle);
            _enemy.OnStateChanged.AddListener(UpdateVisualByState);
        }

        public void Initialize() {
            _animator = _enemy.GetComponentInChildren<Animator>();
        }

        private void HealthChangeHandle(float newHealth) {
            if (_healthBar != null) {
                _healthBar.SetProgress(newHealth);
            }
        }

        public void Dispose() {
            if (_healthBar != null) {
                _enemy.OnHealthChanged.RemoveListener(HealthChangeHandle);
            }

            _healthBar.Dispose();
        }

        private void UpdateVisualByState(Enemy.EnemyState state) {
            SetAnimation(state);
            switch (state) {
                case Enemy.EnemyState.Attack:

                    break;
                case Enemy.EnemyState.Run:

                    break;
                case Enemy.EnemyState.Dead:
                    Dispose();
                    break;
            }
        }

        private void SetAnimation(Enemy.EnemyState state) {
            if (_animator == null) {
                return;
            }
            
            _animator.ResetTrigger(_animationAttack);
            _animator.ResetTrigger(_animationTakeDamage);
            _animator.ResetTrigger(_animationDead);

            if (state == Enemy.EnemyState.Attack) {
                _animator.SetTrigger(_animationAttack);
            }
            
            if (state == Enemy.EnemyState.TakeDamage) {
                _animator.SetTrigger(_animationTakeDamage);
            }
            
            if (state == Enemy.EnemyState.Dead) {
                _animator.SetTrigger(_animationDead);
            }

            _animator.SetBool(_animationRun, state == Enemy.EnemyState.Run);
        }
    }
}