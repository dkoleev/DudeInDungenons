using UnityEngine;

namespace Runtime.UI.Base {
    public class UiBase : MonoBehaviour {
        protected GameController GameController;
        protected ItemsReference ItemsReference;

        public virtual void Initialize(GameController gameController, ItemsReference itemsReference) {
            GameController = gameController;
            ItemsReference = itemsReference;
        }
    }
}