using Avocado.Framework.Patterns.StateMachine;
using Runtime.Logic.States.Player;
using Runtime.Ui.World;
using UnityEngine;

namespace Runtime {
    public class PlayerVisual {
        private static readonly int _animationRun = Animator.StringToHash("Run");
        private static readonly int _animationIdle = Animator.StringToHash("Idle");
        private static readonly int _animationAttack = Animator.StringToHash("Attack");
        private static readonly int _animationTakeDamage= Animator.StringToHash("TakeDamage");
        private static readonly int _animationDead= Animator.StringToHash("Dead");
        
        private readonly WorldBar _healthBar;
        private readonly Player _player;
        private Animator _animator;
        
        public PlayerVisual(Player player) {
            _player = player;
            
            _player.OnStateChanged.AddListener(UpdateVisualByState);
        }
        
        public void Initialize() {
            _animator = _player.GetComponentInChildren<Animator>();
        }

        public void Update() {
            _animator.SetBool(_animationRun, _player.IsMoving);
        }

        public void UpdateVisualByState(IState prevState, IState newState) {
            _animator.ResetTrigger(_animationAttack);
            if (newState is PlayerAttackState) {
                _animator.SetTrigger(_animationAttack);
            }
        }
    }
}
