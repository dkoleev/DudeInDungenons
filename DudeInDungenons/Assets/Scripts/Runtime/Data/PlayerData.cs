using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _speedMove;

        public int MaxHealth => _maxHealth;
        public float SpeedMove => _speedMove;
    }
}
