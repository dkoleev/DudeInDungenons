using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Items {
    [Serializable]
    public class ItemStack {
        /*[TableColumnWidth(35, false)]
        [PropertyOrder(1),PreviewField(Height = 30), ShowInInspector]
        public Texture2D _icon => _item.Icon;*/
        [SerializeField, InlineEditor, PropertyOrder(2)]
        private Item _item;
        [SerializeField, PropertyOrder(3)]
        private int _amount;
        [SerializeField, TableColumnWidth(50, false), PropertyOrder(4)]
        private bool _equipped;

        public Item Item => _item;
        public int Amount => _amount;
        public bool Equipped => _equipped;
    }
}