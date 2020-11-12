using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [Serializable]
    public class ItemStack {
        [SerializeField]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        private Item _item;
        [SerializeField]
        private int _amount;

        public Item Item => _item;
        public int Amount => _amount;
    }
}