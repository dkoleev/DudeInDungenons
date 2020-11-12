using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject {
        [SerializeField]
        private int _maxHealth;

        [SerializeField]
        private ItemStack[] _drop;

        public int MaxHealth => _maxHealth;
        public ItemStack[] Drop => _drop;
    }
}