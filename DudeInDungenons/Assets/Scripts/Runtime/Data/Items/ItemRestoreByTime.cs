using Runtime.Logic.Core.BaseTypes;
using Runtime.Logic.Core.Price;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [CreateAssetMenu(fileName = "Item_Action", menuName = "Data/Items/Item restored by time")]
    public class ItemRestoreByTime : Item {
        [SerializeField]
        private int _maxAmount;
        [SerializeField, Title("Restore time in seconds", horizontalLine:false)]
        private TargetTime _restoreTime;
        [SerializeField]
        private Price _price;

        public int MaxAmount => _maxAmount;
        public TargetTime RestoreTime => _restoreTime;
        public Price Price => _price;
    }
}