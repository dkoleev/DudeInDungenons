using Runtime.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    public enum WeaponType {
        Hand,
        Bit,
        Pistol
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapons/Weapon")]
    public class WeaponData : Item {
        [SerializeField, Title("Animator select player animation by this index")]
        [ValueDropdown("AnimatorIds")]
        private int _animatorId;
        [SerializeField] private int _damage;
        [SerializeField] private float _shootDelay;
        [SerializeField] private float _range;

        public int Damage => _damage;
        public float ShootDelay => _shootDelay;
        public float Range => _range;
        public int AnimatorId => _animatorId;
        
        private static int[] AnimatorIds = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    }
}