using System.Collections.Generic;
using Runtime.Data.Items;
using Runtime.Logic.WeaponSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data {
    [CreateAssetMenu(fileName = "Enemy", menuName = "Data/Enemy")]
    public class EnemyData : ScriptableObject {
        [SerializeField]
        private int _maxHealth;
        [SerializeField]
        private float _speedMove;
        [SerializeField]
        private float _speedRotate;

        [SerializeField, AssetsOnly]
        [InlineEditor(InlineEditorModes.LargePreview)]
        private Weapon _weapon;

        [SerializeField]
        [TableList]
        private List<ItemStack> _drop;

        public int MaxHealth => _maxHealth;
        public float SpeedMove => _speedMove;
        public float SpeedRotate => _speedRotate;
        public List<ItemStack> Drop => _drop;
    }
}