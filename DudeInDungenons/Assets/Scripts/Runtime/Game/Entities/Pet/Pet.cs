using Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Game.Entities.Pet {
    public class Pet : Entity {
        [SerializeField, Required]
        private PetData _data;

        public PetData Data => _data;
        public int CurrentHealth => _model.CurrentHealth;
        
        private PetLogic _model;
        private PetVisual _visual;

        public override void Initialize(GameController gameController) {
            base.Initialize(gameController);
            
            _model = new PetLogic();
            _model.Initialize();
            _visual = new PetVisual(this);
            _visual.Initialize();
        }
    }
}