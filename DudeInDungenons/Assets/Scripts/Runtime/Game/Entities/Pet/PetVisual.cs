using System;
using Runtime.Logic.Core.BaseTypes;
using Runtime.Ui.World;
using UnityEngine;

namespace Runtime.Game.Entities.Pet {
    public class PetVisual : IInitializable, IDisposable {
        private static readonly int Sitting = Animator.StringToHash("Sitting");
        
        private readonly Pet _entity;
        private  WorldBar _healthBar;
        private Animator _animator;

        public PetVisual(Pet entity) {
            _entity = entity;
        }

        public void Initialize() {
            _healthBar = _entity.GetComponentInChildren<WorldBar>();
            if (_healthBar != null) {
                _healthBar.Initialize(_entity.CurrentHealth, _entity.Data.Damage);
            }
            _animator = _entity.GetComponentInChildren<Animator>();
        }

        public void Dispose() {
            
        }
    }
}