using UnityEngine;

namespace Runtime.UI.Base {
    public class UiBase : MonoBehaviour {
        protected GameController GameController;
        protected ItemsReference ItemsReference;

        public bool Initialized { get; protected set; }

        public virtual void Initialize(GameController gameController, ItemsReference itemsReference) {
            GameController = gameController;
            ItemsReference = itemsReference;
        }

        public virtual void SetActive(bool isActive) {
            if (gameObject.activeSelf == isActive) {
                return;
            }

            gameObject.SetActive(isActive);
        }
    }
}