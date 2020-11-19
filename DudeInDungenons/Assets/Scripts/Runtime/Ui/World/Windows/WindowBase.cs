using Sigtrap.Relays;
using UnityEngine;

namespace Runtime.Ui.World.Windows {
    public class WindowBase : MonoBehaviour {
        public Relay OnHide = new Relay();
        public Relay OnShow = new Relay();
        
        public void Show() {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            
            OnShow.Dispatch();
        }

        protected void Hide() {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            
            OnHide.Dispatch();
        }
    }
}