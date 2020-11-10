using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject {
        [SerializeField]
        private int _maxHealth;

        public int MaxHealth => _maxHealth;
    }
}