using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class WeaponData : ScriptableObject {
        [SerializeField] private int _damage;
        [SerializeField] private float _shootDelay;

        public int Damage => _damage;
        public float ShootDelay => _shootDelay;
    }
}