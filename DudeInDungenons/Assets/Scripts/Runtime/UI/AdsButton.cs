using Runtime.UI.Base;
using Sigtrap.Relays;
using UnityEngine.Advertisements;

namespace Runtime.UI {
    public class AdsButton : UiButton {
        public Relay OnAdsCompleted = new Relay();
        
        protected override void Awake() {
            base.Awake();
            
            SetEnabled(AdsManager.Instance.IsReady());
            
            Button.onClick.AddListener(() => {
                AdsManager.Instance.ShowRewardedVideo();
            });

            AdsManager.Instance.OnAdsReady.AddListener(() => {
                SetEnabled(true);
            });

            AdsManager.Instance.OnAdsFinished.AddListener(result => {
                if (result == ShowResult.Finished) {
                    OnAdsCompleted.Dispatch();
                }
            });
        }
    }
}