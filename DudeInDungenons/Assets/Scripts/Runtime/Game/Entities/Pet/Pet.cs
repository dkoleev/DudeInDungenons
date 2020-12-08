using Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Game.Entities.Pet {
    public class Pet : Entity {
        [SerializeField, Required]
        private PetData _data;

        public AssetReference Asset => _data.Asset;
    }
}