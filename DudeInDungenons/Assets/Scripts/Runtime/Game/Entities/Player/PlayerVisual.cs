using Avocado.Framework.Patterns.StateMachine;
using Runtime.Logic.States.Player;
using Runtime.Ui.World;
using UnityEngine;

namespace Runtime.Game.Entities.Player {
    public class PlayerVisual {
        private static readonly int _animationRun = Animator.StringToHash("Run");
        private static readonly int _animationIdle = Animator.StringToHash("Idle");
        private static readonly int _animationAttack = Animator.StringToHash("Attack");
        private static readonly int _animationTakeDamage= Animator.StringToHash("TakeDamage");
        private static readonly int _animationDead= Animator.StringToHash("Dead");
        private static readonly int _animationWeaponId = Animator.StringToHash("Weapon_int");
        
        private readonly WorldBar _healthBar;
        private readonly Player _player;
        private Animator _animator;

        private bool _initialized;
        
        public PlayerVisual(Player player) {
            _player = player;
            
            _player.OnStateChanged.AddListener(UpdateVisualByState);
        }
        
        public void Initialize() {
            _animator = _player.GetComponentInChildren<Animator>();
            
            _initialized = true;
        }

        public void Update() {
            if (!_initialized) {
                return;
            }

            _animator.SetBool(_animationRun, _player.IsMoving);
            var haveWeapon = _player.AttackComponent != null && _player.AttackComponent.CurrentWeapon != null;
            _animator.SetInteger(_animationWeaponId,  value: haveWeapon ? _player.AttackComponent.CurrentWeapon.AnimatorId : 0);
        }

        public void UpdateVisualByState(IState prevState, IState newState) {
            if (!_initialized) {
                return;
            }

            _animator.ResetTrigger(_animationAttack);
            if (newState is PlayerAttackState) {
                _animator.SetTrigger(_animationAttack);
            }
        }
    }
}
