using System.Collections.Generic;
using Runtime.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.UI.MainMenu {
    
    public class Shop : UiBase {
        [SerializeField, Required]
        private List<ShopItem> _items = new List<ShopItem>();

        public override void Initialize(GameController gameController, ItemsReference itemsReference) {
            base.Initialize(gameController, itemsReference);

            _items.ForEach(item => item.Initialize(GameController, ItemsReference));
        }
    }
}