using Runtime.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "Player", menuName = "Data/Player")]
    public class PlayerData : ScriptableObject {
        [SerializeField] 
        [TabGroup("Base")]
        private int _maxHealth;
        [SerializeField] 
        [TabGroup("Base")]
        private float _speedMove;
        [SerializeField] 
        [TabGroup("Base")]
        private float _speedRotate;
        [SerializeField]
        [TabGroup("Starting Inventory")]
        private ItemStack[] _startInventory;
        

        public int MaxHealth => _maxHealth;
        public float SpeedMove => _speedMove;
        public float SpeedRotate => _speedRotate;
    }
}
