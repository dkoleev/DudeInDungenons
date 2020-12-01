using Runtime.Logic.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [CreateAssetMenu(fileName = "Item_Action", menuName = "Data/Items/Action Item")]
    public class ItemAction : Item {
        [SerializeField, Required]
        private Action _action;

        public Action Action => _action;
    }
}