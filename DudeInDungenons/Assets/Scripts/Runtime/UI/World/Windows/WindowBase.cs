using Runtime.UI.Base;
using Sigtrap.Relays;
using UnityEngine;

namespace Runtime.Ui.World.Windows {
    public class WindowBase : UiBase {
        public Relay OnHide = new Relay();
        public Relay OnShow = new Relay();
        
        public virtual void Show() {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            
            OnShow.Dispatch();
        }

        protected virtual void Hide() {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            
            OnHide.Dispatch();
        }
    }
}