using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI.Animations {
    [DisallowMultipleComponent]
    public class ScaleTapAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField]
        private Ease _easyIn = Ease.Linear;
        [SerializeField]
        private float _inDuration;
        [SerializeField]
        private float _inScale;
        [SerializeField]
        private Ease _easyOut = Ease.Linear;
        [SerializeField]
        private float _outDuration;
        [SerializeField]
        private bool _selfTarget;
        [SerializeField]
        [HideIf("_selfTarget")]
        private Transform _target;

        private Transform _currentTarget;
        private float _defaultScale;

        private void Awake() {
            if (_target == null) {
                _currentTarget = transform;
            } else {
                _currentTarget = _target;
            }

            SetDefaultScale(_currentTarget.localScale.x);
        }

        public void SetDefaultScale(float scale) {
            _defaultScale = scale;
        }

        public void OnPointerDown(PointerEventData eventData) {
            _currentTarget.DOScale(new Vector3(_inScale, _inScale), _inDuration).SetEase(_easyIn);
        }

        public void OnPointerUp(PointerEventData eventData) {
            _currentTarget.DOScale(_defaultScale, _outDuration).SetEase(_easyOut);
        }
    }
}