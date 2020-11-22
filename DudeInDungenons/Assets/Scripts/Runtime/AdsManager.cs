using Runtime.Logic.Core;
using Sigtrap.Relays;
using UnityEngine.Advertisements;

namespace Runtime {
    public class AdsManager : SingletonBase<AdsManager>, IUnityAdsListener {
        public Relay<ShowResult> OnAdsFinished = new Relay<ShowResult>();
        public Relay OnAdsRewardReady = new Relay();
        public Relay OnAdsReady = new Relay();
        
        private string _rewardedVideoPlacementId = "rewardedVideo";
        private string _videoPlacementId = "video";

        private bool _initialized;
            
        private AdsManager() {
            if (!_initialized) {
                Initialize();
            }
        }

        public void Initialize() {
            Advertisement.AddListener(this);

#if UNITY_EDITOR
            Advertisement.Initialize("3911161", true);
#elif UNITY_ANDROID
            Advertisement.Initialize("3911161", false);
#elif UNITY_IOS
            Advertisement.Initialize("3911160", false);
#endif
            _initialized = true;
        }

        public void ShowRewardedVideo() {
            Advertisement.Show(_rewardedVideoPlacementId);
        }
        
        public void ShowVideo() {
            Advertisement.Show(_videoPlacementId);
        }

        public bool IsReady() {
            return Advertisement.IsReady(_rewardedVideoPlacementId);
        }

        public void OnUnityAdsReady(string placementId) {
            if (placementId == _rewardedVideoPlacementId) {
                OnAdsRewardReady.Dispatch();
            }else if (placementId == _videoPlacementId) {
                OnAdsReady.Dispatch();
            }
        }

        public void OnUnityAdsDidError(string message) {
            
        }

        public void OnUnityAdsDidStart(string placementId) {
            
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
            OnAdsFinished.Dispatch(showResult);
        }
    }
}