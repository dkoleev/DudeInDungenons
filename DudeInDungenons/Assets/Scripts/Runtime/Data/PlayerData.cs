using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedRotate;

        public int MaxHealth => _maxHealth;
        public float SpeedMove => _speedMove;
        public float SpeedRotate => _speedRotate;
    }
}
