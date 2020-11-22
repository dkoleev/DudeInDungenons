using Runtime.Logic.GameProgress;
using UnityEngine;

namespace Runtime.Ui {
    public class UiBase : MonoBehaviour {
        protected GameController GameController;
        protected GameProgress Progress;
        protected ItemsReference ItemsReference;

        public virtual void Initialize(GameProgress progress, ItemsReference itemsReference) {
            Progress = progress;
            ItemsReference = itemsReference;
        }
    }
}