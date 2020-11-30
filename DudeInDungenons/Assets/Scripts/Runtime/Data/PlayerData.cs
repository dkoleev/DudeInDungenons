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
        [TabGroup("Base")]
        private float _speedRotateNoMove;
        [SerializeField]
        [TabGroup("Base")]
        private PlayerSkinData _startSkin;
        [SerializeField]
        [TabGroup("Starting Inventory")]
        [TableList]
        private PLayerStartInventoryItemStack[] _startInventory;

        public int MaxHealth => _maxHealth;
        public float SpeedMove => _speedMove;
        public float SpeedRotate => _speedRotate;
        public float SpeedRotateNoMove => _speedRotateNoMove;
        public PlayerSkinData StartSkin => _startSkin;
        public PLayerStartInventoryItemStack[] StartInventory => _startInventory;
    }
}
