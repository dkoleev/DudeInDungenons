using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [CreateAssetMenu(fileName = "Item_Action", menuName = "Data/Items/Item restored by time")]
    public class ItemRestoreByTime : Item {
        [SerializeField]
        private int _maxAmount;
        [SerializeField, Title("Restore time in seconds", horizontalLine:false)]
        private long _restoreTime;

        public int MaxAmount => _maxAmount;
        public long RestoreTime => _restoreTime;
    }
}