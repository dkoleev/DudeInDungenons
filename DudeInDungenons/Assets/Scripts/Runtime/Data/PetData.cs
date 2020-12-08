using Runtime.Data.Items;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "Pet", menuName = "Data/Pet")]
    public class PetData : ItemAction {
        [SerializeField]
        private float _speedMove;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private int _maxHealth;

        public float SpeedMove => _speedMove;
        public float Damage => _damage;
        public int MaxHealth => _maxHealth;
    }
}