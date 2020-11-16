using Runtime.Data.Items;
using UnityEngine;

namespace Runtime.Data {
    public enum WeaponType {
        Hand,
        Bit,
        Pistol
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapons/Weapon")]
    public class WeaponData : Item {
        [SerializeField] private int _damage;
        [SerializeField] private float _shootDelay;
        [SerializeField] private float _range;

        public int Damage => _damage;
        public float ShootDelay => _shootDelay;
        public float Range => _range;
    }
}