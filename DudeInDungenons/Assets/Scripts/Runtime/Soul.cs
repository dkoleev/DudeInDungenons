using DG.Tweening;
using Runtime.Logic.Core.EventBus;
using Runtime.Logic.Events;
using UnityEngine;

namespace Runtime {
    public class Soul : MonoBehaviour {
        private Tweener _moveTweener;
        private void Start() {
            gameObject.gameObject.SetActive(false);
        }
        
        public void Activate() {
            transform.parent = null;
            gameObject.SetActive(true);
            
            EventBus<OnSoulCreated>.Raise(new OnSoulCreated(this));
        }

        public void MoveTo(Transform target) {
            _moveTweener = transform.DOMove(target.position, 0.5f).SetEase(Ease.InCubic);
            _moveTweener.onComplete += DoDestroy;
        }

        private void DoDestroy() {
            if (_moveTweener?.onComplete != null) {
                _moveTweener.onComplete -= DoDestroy;
            }

            Destroy(gameObject);
        }
    }
}