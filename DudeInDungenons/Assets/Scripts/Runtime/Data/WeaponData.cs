using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class WeaponData : ScriptableObject {
        [SerializeField] private int _damage;
        [SerializeField] private float _shootDelay;
        [SerializeField] private float _range;

        public int Damage => _damage;
        public float ShootDelay => _shootDelay;
        public float Range => _range;
    }
}