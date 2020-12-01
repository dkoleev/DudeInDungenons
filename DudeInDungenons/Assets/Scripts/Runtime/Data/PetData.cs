using Runtime.Data.Items;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "Pet", menuName = "Data/Pet")]
    public class PetData : ItemAction {
        private float _speedMove;
        [SerializeField]
        private float _damage;

        public float SpeedMove => _speedMove;
        public float Damage => _damage;
    }
}