using Runtime.Logic.Components;
using UnityEngine.AI;

namespace Runtime.Logic.States.Ai {
    public class AiAttack : AiBase {
        private AttackComponent _attackComponent;
        private IDamagable _target;

        public AiAttack(NavMeshAgent agent, AttackComponent attackComponent, IDamagable target) : base(agent) {
            _attackComponent = attackComponent;
            _target = target;
        }
        
        public override void Enter() {
            base.Enter();
            
            _attackComponent?.Reset();
            Agent.isStopped = true;
        }

        public override void Tick() {
            base.Tick();
            
            _attackComponent?.Update(_target);
        }
    }
}